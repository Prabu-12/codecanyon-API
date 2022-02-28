using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using QuizplusApi.ViewModels.Question;

namespace QuizplusApi.Services
{
    public class UploadService
    {
        public List <QuestionUpload> ReadCSVFile(string location)
        {
            try
            {
                using(var reader = new StreamReader(location))
                using(var csv=new CsvReader(reader,CultureInfo.InstalledUICulture))
                {
                    csv.Context.RegisterClassMap<QuestionUploadMap>();
                    var records=csv.GetRecords<QuestionUpload>().ToList();
                    return records;
                }
            }
            catch (Exception e) 
            {  
                throw new Exception(e.Message);  
            }
        }
    }
}