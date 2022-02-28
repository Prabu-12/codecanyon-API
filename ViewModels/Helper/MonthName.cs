namespace QuizplusApi.ViewModels.Helper
{
    public static class MonthName
    {
        public static string ReturnMonthName(int number)
        {
            if(number==1)
            {
                return "January";
            }
            else if(number==2)
            {
                return "February";
            }
            else if(number==3)
            {
                return "March";
            }
            else if(number==4)
            {
                return "April";
            }
            else if(number==5)
            {
                return "May";
            }
            else if(number==6)
            {
                return "June";
            }
            else if(number==7)
            {
                return "July";
            }
            else if(number==8)
            {
                return "August";
            }
            else if(number==9)
            {
                return "September";
            }
            else if(number==10)
            {
                return "October";
            }
            else if(number==11)
            {
                return "November";
            }
            else if(number==12)
            {
                return "December";
            }
            else
            {
                return "";
            }
        }
    }
}