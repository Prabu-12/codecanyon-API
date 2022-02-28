using CsvHelper.Configuration;

namespace QuizplusApi.ViewModels.Question
{
    public sealed class QuestionUploadMap:ClassMap<QuestionUpload>
    {
        public QuestionUploadMap(){
            Map(x=>x.QuestionDetail).Name("questionDetail");
            Map(x=>x.PerQuestionMark).Name("perQuestionMark");
            Map(x=>x.QuestionTypeId).Name("questionTypeId");
            Map(x=>x.QuestionLavelId).Name("questionLavelId");
            Map(x=>x.QuestionCategoryId).Name("questionCategoryId");
            Map(x=>x.OptionA).Name("optionA");
            Map(x=>x.OptionB).Name("optionB");
            Map(x=>x.OptionC).Name("optionC");
            Map(x=>x.OptionD).Name("optionD");
            Map(x=>x.OptionE).Name("optionE");
            Map(x=>x.CorrectOption).Name("correctOption");
            Map(x=>x.AnswerExplanation).Name("answerExplanation");
        }
    }
}