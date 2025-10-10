using AutoFocus.Enums;
using AutoFocus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFocus.Config
{
    /// <summary>
    /// Application configuration manager
    /// </summary>
    public static class ConfigurationManager
    {
        /// <summary>
        /// Application configuration settings
        /// </summary>
        public class AppConfig
        {
            // Processing settings
            public int KernelSize { get; set; } = 3;
            public bool UseParallel { get; set; } = true;
            public bool EnableSIMD { get; set; } = true;
            public bool SaveIntermediateResults { get; set; } = false;
            
            // Default selections
            public FocusMeasureType DefaultMeasure { get; set; } = FocusMeasureType.Tenengrad;
            public SearchStrategyType DefaultStrategy { get; set; } = SearchStrategyType.Sequential;
            public Rect DefaultROI { get; set; } = new Rect(0, 0, 1, 1);
            
            // File paths
            public string LastFolderPath { get; set; } = "";
            public string ExportPath { get; set; } = "";
            
            // UI settings
            public bool ShowColormap { get; set; } = false;
            public bool AutoSwitchToResults { get; set; } = true;
            
            // Performance settings
            public int MaxParallelThreads { get; set; } = Environment.ProcessorCount;
            public int ImageCacheSize { get; set; } = 10;
        }
        
        private const string ConfigFile = "autofocus_config.json";
        
        public static void SaveConfig(AppConfig config)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(config, 
                    new System.Text.Json.JsonSerializerOptions 
                    { 
                        WriteIndented = true 
                    });
                System.IO.File.WriteAllText(ConfigFile, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save configuration: {ex.Message}", ex);
            }
        }
        
        public static AppConfig LoadConfig()
        {
            try
            {
                if (System.IO.File.Exists(ConfigFile))
                {
                    var json = System.IO.File.ReadAllText(ConfigFile);
                    return System.Text.Json.JsonSerializer.Deserialize<AppConfig>(json) 
                        ?? new AppConfig();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load configuration: {ex.Message}");
            }
            
            return new AppConfig();
        }
        
        public static void DeleteConfig()
        {
            try
            {
                if (System.IO.File.Exists(ConfigFile))
                {
                    System.IO.File.Delete(ConfigFile);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete configuration: {ex.Message}", ex);
            }
        }
    }
    
}