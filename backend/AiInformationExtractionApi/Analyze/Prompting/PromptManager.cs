using System;
using System.Collections.Generic;
using Light.EmbeddedResources;
using OpenAI.Chat;
using Shared.Messages.Analyze;

namespace AiInformationExtractionApi.Analyze.Prompting;

public sealed class PromptManager
{
    private const string ExamplePersonalData =
        """
        An example could look like this: "Hello, my name is Jon Doe, I live at 58 Main Street, 9245 Smalltown, my
        insurance ID is QY-3942890, I was born on 06/23/1992, my phone number is +31 32482-3996, my email is
        john.doe@gmail.com, and my license plate is 6CB-724".

        The correct answer here would be:

        ```json
        {
            "firstName": "Jon",
            "lastName": "Doe",
            "street": "58 Main Street",
            "zipCode": "9245",
            "location": "Smalltown",
            "insuranceId": "QY-3942890",
            "dateOfBirth": "1992-06-23T00:00:00Z",
            "telephone": "+31 32482-3996",
            "email": "john.doe@gmail.com",
            "licensePlate": "6CB-724"
        }
        ```
        """;

    private const string ExampleVehicleDamage =
        """
        Here is an example: "The front bumper is scratched, the left headlight is broken, the right door is dented,
        The back left tire is popped."

        The correct answer here is:

        ```json
        {
            "front": "Scratch",
            "frontLeft: "TotalLoss",
            "midRight": "Bump",
            "rearLeftTire": "TotalLoss"
        }
        """;

    private const string ExampleCircumstances =
        """
        Here is an example: "The accident happened on 2022-01-15 at 3 in the afternoon, it was a car accident, me and my wife Carol
        Smith were in the car, we were on our way to the grocery store in our blue Audi A4."

        You should extract the following information from this this:

        ```json
        {{
             "dateOfAccidentUtc": "2022-01-15T15:00:00Z",
             "accidentType": "CarAccident",
             "ReasonOfTravel": "Drive to Grocery Store",
             "CarType": "Audi A4",
             "CarColor": "Blue",
             "Passengers": [
                 {
                     "firstName": "Martin",
                     "lastName": "Smith"
                 },
                 {
                     "firstName": "Carol",
                     "lastName": "Smith"
                 }
             ]
        }}
        ```

        Please note that you can use the already existing personal data to fill the first passenger.
        """;

    private readonly Dictionary<(AnalysisType, FormSection), string> _prompts;

    public PromptManager()
    {
        var personalDataJsonSchema = this.GetEmbeddedResource("PersonalData.schema.json");
        var circumstancesJsonSchema = this.GetEmbeddedResource("Circumstances.schema.json");
        var vehicleDamageJsonSchema = this.GetEmbeddedResource("VehicleDamage.schema.json");

        _prompts = new Dictionary<(AnalysisType, FormSection), string>
        {
            [(AnalysisType.Text, FormSection.PersonalData)] =
                CreateTextSystemPrompt(personalDataJsonSchema, ExamplePersonalData),
            [(AnalysisType.Text, FormSection.Circumstances)] =
                CreateTextSystemPrompt(circumstancesJsonSchema, ExampleCircumstances),
            [(AnalysisType.Text, FormSection.VehicleDamage)] =
                CreateTextSystemPrompt(vehicleDamageJsonSchema, ExampleVehicleDamage),
            [(AnalysisType.Image, FormSection.PersonalData)] =
                CreateImageSystemPrompt(personalDataJsonSchema),
            [(AnalysisType.Image, FormSection.Circumstances)] =
                CreateImageSystemPrompt(circumstancesJsonSchema),
            [(AnalysisType.Image, FormSection.VehicleDamage)] =
                CreateImageSystemPrompt(vehicleDamageJsonSchema)
        };
    }

    public List<ChatMessage> CreateTextAnalysisPrompt(
        FormSection formSection,
        string contentToAnalyze,
        string? existingInformation = null
    )
    {
        var systemPrompt = _prompts[(AnalysisType.Text, formSection)];

        var chatMessages = new List<ChatMessage> { new SystemChatMessage(systemPrompt) };
        if (existingInformation != null)
        {
            chatMessages.Add(new UserChatMessage(existingInformation));
        }

        chatMessages.Add(new UserChatMessage(contentToAnalyze));

        return chatMessages;
    }

    public List<ChatMessage> CreateImageAnalysisPrompt(
        FormSection formSection,
        byte[] image,
        string imageMediaType,
        string? existingInformation = null
    )
    {
        var imageData = new BinaryData(image);
        var messageContentPart = ChatMessageContentPart.CreateTextPart("Please analyze the image");
        var imageContentPart = ChatMessageContentPart.CreateImagePart(imageData, imageMediaType);
        var systemPrompt = _prompts[(AnalysisType.Image, formSection)];
        var chatMessages = new List<ChatMessage> { new SystemChatMessage(systemPrompt) };
        if (existingInformation != null)
        {
            chatMessages.Add(new UserChatMessage(existingInformation));
        }
        chatMessages.Add(new UserChatMessage(messageContentPart, imageContentPart));
        
        return chatMessages;
    }

    private static string CreateTextSystemPrompt(string jsonSchema, string example) =>
        $"""
         Your job is to analyze text and extract information out of it. The context is a complex vehicle damage
         report form for insurances. The information you should extract is represented by the following JSON schema:

         ```json
         {jsonSchema}
         ```

         Your answer should be in JSON format, according to the rules defined in the JSON schema. It is possible that
         the text will not contain all the information represented by the JSON schema. You can simply leave out missing
         information. If you cannot find any information, just return an empty JSON object.

         {example}
         """;
    
    private static string CreateImageSystemPrompt(string jsonSchema) =>
        $"""
         Your job is to analyze an image and extract information out of it. The context is a complex vehicle damage
         report form for insurances. The information you should extract is represented by the following JSON schema:

         ```json
         {jsonSchema}
         ```

         Your answer should be in JSON format, according to the rules defined in the JSON schema. It is possible that
         the text will not contain all the information represented by the JSON schema. You can simply leave out missing
         information. If you cannot find any information, just return an empty JSON object.
         """;
}