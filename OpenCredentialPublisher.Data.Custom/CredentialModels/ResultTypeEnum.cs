using System.Runtime.Serialization;

namespace OpenCredentialPublisher.Data.Custom.CredentialModels
{
    public enum ResultTypeEnum
    {
        [EnumMember(Value = "GradePointAverage")] GradePointAverage,
        [EnumMember(Value = "LetterGrade")] LetterGrade,
        [EnumMember(Value = "Percent")] Percent,
        [EnumMember(Value = "PerformanceLevel")] PerformanceLevel,
        [EnumMember(Value = "PredictedScore")] PredictedScore,
        [EnumMember(Value = "RawScore")] RawScore,
        [EnumMember(Value = "Result")] Result,
        [EnumMember(Value = "RubricCriterion")] RubricCriterion,
        [EnumMember(Value = "RubricCriterionLevel")] RubricCriterionLevel,
        [EnumMember(Value = "RubricScore")] RubricScore,
        [EnumMember(Value = "ScaledScore")] ScaledScore,
        [EnumMember(Value = "Status")] Status
    }
}