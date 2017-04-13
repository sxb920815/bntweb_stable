using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;

namespace BntWeb.Article.ApiModels
{
    public class ArticleModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Blurb { get; set; }

        public DateTime CreateTime { get; set; }

        public List<SimplifiedStorageFile> Images { set; get; }

        public ArticleModel(Models.Article model, List<StorageFile> imgList)
        {
            Id = model.Id;
            Title = model.Title;
            Blurb = model.Blurb;
            CreateTime = model.CreateTime;

            Images = imgList.Select(me => me.Simplified()).ToList(); 
        }
    }
}