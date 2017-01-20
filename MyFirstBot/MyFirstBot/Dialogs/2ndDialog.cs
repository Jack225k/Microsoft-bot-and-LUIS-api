using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Luis.Models;
using System.Net.Http;
using MyFirstBot.model;

namespace MyFirstBot.Dialogs
{

    [LuisModel("4c9a82f8-bcc4-4472-9cb3-46321b6055e2", "e686ad6ca03b4cf29d26288783935568")]
    [Serializable]

    // create new class, inherit properties from the microsoft BOTNET framework & LUIS.

    public class _2ndDialog : LuisDialog<object>
    {
        public _2ndDialog()
        {
            //
        }

        public _2ndDialog(ILuisService service) : base(service)
        {
            //
        }

        //Default LUIS intent returns "Im sorry I didn't understand you". This applies to exeptions and values that have not been set to an intent in the LUIS platform. 

        [LuisIntent("")]

        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry. I didn't understand you.");

            context.Wait(MessageReceived);
        }

        //The method is called once the trending movies intent has been pushed from LUIS Api. 

        [LuisIntent("intent.movie.trending")]
        public async Task TrenMovie(IDialogContext context, LuisResult result)
        {
            /*

            If you want to display a message then use this string and add the "message" variable to the asyc return line. 

                string message = $"*Trending  movies:*" + $"{Environment.NewLine }{ Environment.NewLine }>" + $"Thank You For searching!!";
                await context.PostAsync(message);
                context.Wait(MessageReceived);

            */
            //Declare variables that need to be received and stored from the trackt.tv api. 
            //We are not using these values as we only require them for the text output. With the template we are pulling directly from the deserializer. 

            //long trendingYear = 0; long movieWatchers = 0; 
            //string trendingMovies = "";
            //int count = 1,

            //If you wanted to retun more movies then just add more vaiables. There are definately better ways of doing this process. I will make a for loop and array to handle this in the future. 
            string title1 = "", title2 = "", title3 = "", title4 = "", title5 = "";
            int year1 = 0, year2 = 0, year3 = 0, year4 = 0, year5 = 0 ;

            //Make the connection to the Trakt.tv API. This will be to get the list of movies from them. (You need an account to get the API Key)

            using (var client = new HttpClient { BaseAddress = new Uri("https://api.trakt.tv/") })
            {
                client.DefaultRequestHeaders.Add("trakt-api-key", "c91d0b28dd5176b69740545b01f9f3fc1ef7150a8b7991ed40403706a7daa8e2");

                //Go into the directory of the API to https://api.trakt.tv/movies/treding to get the list of movies that you want it to return. 
                var response = await client.GetAsync("movies/trending");
                //Read the response returned at a string
                var responseString = await response.Content.ReadAsStringAsync();
                //Use of the Newtonsoft deserializer to map the data against your selected Model
                var res = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrendingMovie>>(responseString);
                //assign your values to the variables that were created above. Essentially you are taking the values from the Newsoft deserialized object that was created.
                //Res is the rsult sent back from the Newsoft.Json deserializer. 
                
                title1 = res[0].movie.title.ToString();
                year1 = res[0].movie.year;
                title2 = res[1].movie.title.ToString();
                year2 = res[1].movie.year;
                title3 = res[2].movie.title.ToString();
                year3 = res[2].movie.year;
                title4 = res[3].movie.title.ToString();
                year4 = res[3].movie.year;
                title5 = res[4].movie.title.ToString();
                year5 = res[4].movie.year;
                
                //For loop that returns the list of movies that we pulled from the trakt.tv API in a text format to the Bot. We won't be using this because we are using the display card instead. 

                //for (int i = 0; i < res.Count; i++)
                //{                   
                //    trendingYear = res[i].movie.year;
                //    movieWatchers = res[i].watchers;
                //    trendingMovies += $"{Environment.NewLine }{ Environment.NewLine }" + count + ")" + res[i].movie.title.ToString() + " :" + $"{Environment.NewLine}{Environment.NewLine}"
                //    + $"Year: " + trendingYear + "Watchers: " + movieWatchers;
                //    count = count + 1;
                //}
            }
            //You would uncomment this line if you want the text to display to the screen. 
            //await context.PostAsync(trendingMovies);
            //context.Wait(MessageReceived);

            //Not too sure what this line does. 

            var message = context.MakeMessage();

            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments = GetCardsAttachments(title1, title2, title3, title4, title5, year1, year2, year3, year4, year5);

            //This simply returns a plain hero card, the method is listed below, once again we will not be making use of it. 
            //var attachment = GetHeroCard(title1, year1);
            //message.Attachments.Add(attachment);

            //This takes the message that was created and sends it to the chatBot. 
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        //this is the receipt card attachment by itself. However we won't be making use of this since we will display a hero card within the carousal attachment. 
        //private static Attachment GetReceiptCard(string title1, string title2, string title3, string title4, string title5,  int year1, int year2, int year3, int year4, int year5)
        //{
        //    var receiptCard = new ReceiptCard
        //    {
        //        Title = "Trending movies",
        //        Facts = new List<Fact> { new Fact("Movies are pulled from trackt.tv") },
        //        Items = new List<ReceiptItem>
        //        {
        //            new ReceiptItem(title1, price: year1.ToString(), image: new CardImage(url: "http://www.owensvalleyhistory.com/at_the_movies18/film_reel_l.png")),
        //            new ReceiptItem(title2, price: year2.ToString(), image: new CardImage(url: "http://www.owensvalleyhistory.com/at_the_movies18/film_reel_l.png")),
        //            new ReceiptItem(title3, price: year3.ToString(), image: new CardImage(url: "http://www.owensvalleyhistory.com/at_the_movies18/film_reel_l.png")),
        //            new ReceiptItem(title4, price: year4.ToString(), image: new CardImage(url: "http://www.owensvalleyhistory.com/at_the_movies18/film_reel_l.png")),
        //            new ReceiptItem(title5, price: year5.ToString(), image: new CardImage(url: "http://www.owensvalleyhistory.com/at_the_movies18/film_reel_l.png")),
        //        },
        //        Tax = "$ 7.50",
        //        Total = "Thanks for making your search with our Bot!",


        //        //Buttons = new List<CardAction>
        //        //{
        //        //    new CardAction(
        //        //        ActionTypes.OpenUrl,
        //        //        "More information",
        //        //        "https://account.windowsazure.com/content/6.10.1.38-.8225.160809-1618/aux-pre/images/offer-icon-freetrial.png",
        //        //        "https://azure.microsoft.com/en-us/pricing/")
        //        //}
        //    };

        // return receiptCard.ToAttachment();
        //}

            
        private static IList<Attachment> GetCardsAttachments(string title1, string title2, string title3, string title4, string title5, int year1, int year2, int year3, int year4, int year5)
        {
            return new List<Attachment>()
            {
                GetHeroCard(
                    title1,
                    "Released in " + year1.ToString(),
                    "If you want to find out more about the listed movies click on the link below, it will take you to the website called tract.tv",
                    new CardImage(url: "http://www.owensvalleyhistory.com/at_the_movies18/film_reel_l.png"),
                    new CardAction(ActionTypes.OpenUrl, "Click Here To Get More Info", value: "https://trakt.tv/movies/trending")),

                GetHeroCard(
                    title2,
                    "Released in " + year2.ToString(),
                    "If you want to find out more about the listed movies click on the link below, it will take you to the website called tract.tv",
                    new CardImage(url: "http://www.owensvalleyhistory.com/at_the_movies18/film_reel_l.png"),
                    new CardAction(ActionTypes.OpenUrl, "Click Here To Get More Info", value: "https://trakt.tv/movies/trending")),

                GetHeroCard(
                    title3,
                    "Released in" + year3.ToString(),
                    "If you want to find out more about the listed movies click on the link below, it will take you to the website called tract.tv",
                    new CardImage(url: "http://www.owensvalleyhistory.com/at_the_movies18/film_reel_l.png"),
                    new CardAction(ActionTypes.OpenUrl, "Click Here To Get More Info", value: "https://trakt.tv/movies/trending")),

                GetHeroCard(
                    title4,
                    "Released in" + year4.ToString(),
                    "If you want to find out more about the listed movies click on the link below, it will take you to the website called tract.tv",
                    new CardImage(url: "http://www.owensvalleyhistory.com/at_the_movies18/film_reel_l.png"),
                    new CardAction(ActionTypes.OpenUrl, "Click Here To Get More Info", value: "https://trakt.tv/movies/trending")),

                GetHeroCard(
                    title5,
                    "Released in" + year5.ToString(),
                    "If you want to find out more about the listed movies click on the link below, it will take you to the website called tract.tv",
                    new CardImage(url: "http://www.owensvalleyhistory.com/at_the_movies18/film_reel_l.png"),
                    new CardAction(ActionTypes.OpenUrl, "Click Here To Get More Info", value: "https://trakt.tv/movies/trending")),

            };
        }

        //This code above created the carousal template. I then posted the hero cards inside the carousal. 
        //This is basically the template for the hero card. 
        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            //Simplly returns the template to the carousal template. 
            return heroCard.ToAttachment();
        }
    }
}
        
