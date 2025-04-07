﻿using Hephaistos_Maui.View;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using Hephaistos_Maui.Models;

namespace Hephaistos_Maui
{
    public static class MauiProgram
    {
        
        static string apiBaseUrl = "https://hephaistos-backend-c6c5ewhraedvgzex.germanywestcentral-01.azurewebsites.net/api/";

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

      
        public static async Task<bool> RegisterAsync(string username, string email, string password)
        {
            try
            {
                var registrationData = new
                {
                    username,
                    email,
                    password
                };

                string json = JsonSerializer.Serialize(registrationData);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync($"{apiBaseUrl}Auth/register", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Regisztrációs hiba: {ex.Message}");
                return false;
            }
        }

       
        public static async Task<bool> LoginAsync(string usernameOrEmail, string password, bool stayLoggedIn)
        {
            try
            {
                var loginData = new
                {
                    usernameOrEmail,
                    password,
                    stayLoggedIn
                };

                string json = JsonSerializer.Serialize(loginData);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync($"{apiBaseUrl}Auth/login", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bejelentkezési hiba: {ex.Message}");
                return false;
            }
        }

        public static async Task<List<Subject>?> GetSubjectsAsync()
        {
            try
            {
                using HttpClient client = new();
                string apiUrl = $"{apiBaseUrl}Subjects"; // Az apiBaseUrl már tartalmazza az "/api/"-t

                string json = await client.GetStringAsync(apiUrl);
                var subjects = JsonSerializer.Deserialize<List<Subject>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return subjects;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a tantárgyak lekérésekor: {ex.Message}");
                return null;
            }
        }
        public static async Task<T?> GetFromApiAsync<T>(string endpoint)
        {
            try
            {
                using HttpClient client = new HttpClient();
                var response = await client.GetAsync($"{apiBaseUrl}{endpoint}");

                if (!response.IsSuccessStatusCode)
                    return default;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GET hiba: {ex.Message}");
                return default;
            }
        }
    }
}
