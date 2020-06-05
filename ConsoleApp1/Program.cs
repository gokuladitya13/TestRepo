using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpbrake.Client;
using Sharpbrake.Client.Model;

namespace ConsoleApp1
{
    class Program
    {
        AirbrakeNotifier airbrake;
        public Program()
        {
            airbrake = new AirbrakeNotifier(new AirbrakeConfig
            {
                ProjectId = "275997",
                ProjectKey = "fd4b429bba78c4f190a0b9b799e99cd1",
                LogFile = "airbrake.log",
                Environment = "dev"
            });
        }
        static void Main(string[] args)
        {

            Program obj = new Program();
            obj.SystemException();
            obj.CreateCustomException();
            obj.CustomExceptionWithFilter();
            Console.ReadKey();

        }
        void SystemException()
        {
            try
            {
                Console.WriteLine("running system exception method......");
                Console.WriteLine();
                int a = 0;
                int b = 100 / a;
            }
            catch (Exception ex)
            {
             
                Notice notice = airbrake.BuildNotice(ex);
                var response = airbrake.NotifyAsync(notice).Result;
                Console.WriteLine("exception logged in airbarke dashboard");
                Console.WriteLine("Status: {0}, Id: {1}, Url: {2}", response.Status, response.Id, response.Url);
               
            }

        }
        void CreateCustomException()
        {
            try
            {
                Console.WriteLine("\nrunning custom exception method......");
                Console.WriteLine("\n creating custom exception");
                throw new Exception("new custom exception");
            }
            catch (Exception ex)
            {
                Notice notice = airbrake.BuildNotice(ex);
                var response = airbrake.NotifyAsync(notice).Result;
                Console.WriteLine("exception logged in airbarke dashboard");
                Console.WriteLine("Status: {0}, Id: {1}, Url: {2}", response.Status, response.Id, response.Url);
            }

        }
        void CustomExceptionWithFilter()
        {
            try
            {
                Console.WriteLine("\nrunning system exception method......");
                Console.WriteLine();
                Console.WriteLine("creating custom exception");
                throw new Exception("new exception with filter");
            }
            catch (Exception ex)
            {
                Notice notice = airbrake.BuildNotice(ex);
                notice.Context.User = new UserInfo();
                notice.Context.User.Email = "test@example.com";

                airbrake.AddFilter(n =>{
                    // ignore notice if email
                    if (n.Context.User != null)
                        return null;
                    else
                        return n;

                });
                var response = airbrake.NotifyAsync(notice).Result;
                Console.WriteLine("exception logged in airbarke dashboard");
                Console.WriteLine("Status: {0}, Id: {1}, Url: {2}", response.Status, response.Id, response.Url);
                
            }
        }
    }
}
