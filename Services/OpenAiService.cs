using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VaderSharp2;
using static System.Net.WebRequestMethods;

namespace Fall2025_Project3_jppierre.Services
{
    public class OpenAiService : IAiService
    {
        private readonly Uri _apiEndpoint = new("https://fall2025-aif-eastus2.cognitiveservices.azure.com/");
        private readonly ApiKeyCredential _apiKeyCredential;
        private readonly string _model = "gpt-4.1-nano";
        private readonly SentimentIntensityAnalyzer _analyzer = new();

        public OpenAiService( IConfiguration config)
        {
            var key = config["OpenAI:ApiKey"] ?? throw new InvalidOperationException("OpenAI API key not configured.");
            _apiKeyCredential = new(key);        
        }
            
        public async Task<IList<string>> GenerateReviewsAsync(string movieTitle, int count)
        {
            ChatClient client = new AzureOpenAIClient(_apiEndpoint, _apiKeyCredential).GetChatClient(_model);
            var messages = new ChatMessage[]
            {
                new SystemChatMessage("You are a JSON generator. Output only valid JSON."),
                new UserChatMessage($@"You are to produce a JSON array called ""reviews"" containing exactly {count} short unique movie reviews (1-2 sentences) about the movie ""{movieTitle}"". Only output valid JSON; for example: {{ ""reviews"": [""r1"",""r2"", ...] }}")
            };
            ClientResult<ChatCompletion> response = await client.CompleteChatAsync(messages);
            return ParseArrayFromJson(response, "reviews");
        }

        public async Task<IList<string>> GenerateTweetsAsync(string actorName, int count)
        {
            ChatClient client = new AzureOpenAIClient(_apiEndpoint, _apiKeyCredential).GetChatClient(_model);
            var messages = new ChatMessage[]
            {
                new SystemChatMessage("You are a JSON generator. Output only valid JSON."),
                new UserChatMessage($@"You are to produce a JSON array called ""tweets"" containing exactly {count} short tweets from a variety of users about the actor ""{actorName}"". Each tweet should be in the format ""username: tweet text"" where username is a creative Twitter handle (e.g., @moviefan123, @cinemaphile). Only output valid JSON; for example: {{ ""tweets"": [""@user1: Great actor!"", ""@user2: Love their work!"", ...] }}")
            };
            ClientResult<ChatCompletion> response = await client.CompleteChatAsync(messages);
            return ParseArrayFromJson(response, "tweets");
        }

        public Task<IList<string>> AnalyzeSentimentAsync(IList<string> texts)
        {
            var results = new List<string>(texts.Count);
            foreach (var t in texts)
            {
                var scores = _analyzer.PolarityScores(t ?? string.Empty);
                // compound ranges: typical thresholds: >0.05 positive, < -0.05 negative, else neutral
                var label = scores.Compound switch
                {
                    > 0.05 => "Positive",
                    < -0.05 => "Negative",
                    _ => "Neutral"
                };
                results.Add(label);
            }
            return Task.FromResult((IList<string>)results);
        }

        // --- helper methods ---

        private IList<string> ParseArrayFromJson(ClientResult<ChatCompletion> result, string propertyName)
        {
            try
            {
                // Extract the content from the ChatCompletion response
                var chatCompletion = result.Value;
                var content = chatCompletion.Content[0].Text;
                
                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.TryGetProperty(propertyName, out var arr) && arr.ValueKind == JsonValueKind.Array)
                {
                    var list = new List<string>();
                    foreach (var el in arr.EnumerateArray())
                    {
                        if (el.ValueKind == JsonValueKind.String)
                            list.Add(el.GetString() ?? string.Empty);
                        else
                            list.Add(el.ToString());
                    }
                    return list;
                }
            }
            catch (JsonException)
            {
                // parsing failed — return empty list to avoid crashes; caller should handle empty result
            }
            return new List<string>();
        }
    }
}