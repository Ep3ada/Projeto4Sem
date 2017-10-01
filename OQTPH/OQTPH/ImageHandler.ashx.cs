using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OQTPH
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int id;

            if (int.TryParse(context.Request.QueryString["idimgev"], out id) == false)
            {
                context.Response.StatusCode = 400;
                return;
            }

            //up.SaveAs(Server.MapPath($"~/App_Data/{idEv}-imagem.jpg"));

            //string caminho = context.Server.MapPath(id + "-imagem.jpg");
            string caminho = context.Server.MapPath($"~/App_Data/ImagensEventos/{id}-imagem.jpg");

            if (!System.IO.File.Exists(caminho))
            {
                context.Response.StatusCode = 404;
                return;
            }

            context.Response.ContentType = "image/jpg";
            context.Response.WriteFile(caminho);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}