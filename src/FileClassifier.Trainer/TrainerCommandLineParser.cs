﻿using System.CommandLine;
using System.CommandLine.Invocation;

using FileClassifier.lib.Enums;
using FileClassifier.lib.Options;

namespace FileClassifier.Trainer
{
    public class TrainerCommandLineParser
    {
        public static TrainerCommandLineOptions Parse(string[] args)
        {
            TrainerCommandLineOptions options = null;

            Option oVerbose = new Option(
                "--verbose",
                "Enable verbose output",
                new Argument<bool>(defaultValue: false));

            Option oFolder = new Option(
                "--folderofdata",
                "Folder containing data to be parsed to build the model",
                new Argument<string>());

            Option oModelType = new Option(
                "--modeltype",
                "Model Type",
                new Argument<ModelType>());

            var rootCommand = new RootCommand
            {
                Description = "File Trainer builds a model"
            };

            rootCommand.AddOption(oFolder);
            rootCommand.AddOption(oVerbose);
            rootCommand.AddOption(oModelType);

            rootCommand.TreatUnmatchedTokensAsErrors = true;

            rootCommand.Argument.AddValidator(symbolResult =>
            {
                if (symbolResult.Children["--folderofdata"] is null)
                {
                    return "Folder Path is required";
                }
                else
                {
                    return null;
                }
            });

            rootCommand.Handler = CommandHandler.Create<string, bool, ModelType>((folderPath, verbose, modelType) =>
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    return;
                }

                options = new TrainerCommandLineOptions
                {
                    FolderOfData = folderPath,
                    LogLevel = LogLevels.DEBUG,
                    ModelType = modelType
                };
            });

            rootCommand.InvokeAsync(args).Wait();

            return options;
        }
    }
}