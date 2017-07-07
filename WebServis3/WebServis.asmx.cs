using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace WebServis3
{
    /// <summary>
    /// Summary description for WebServis
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServis : System.Web.Services.WebService
    {      
       
        //login metodu

        [WebMethod]
        public string getapplogin(string tcno, string sifre)
        {
            DataClassesLinqDataContext db = new DataClassesLinqDataContext();
            var sql = from applogin in db.applogins
                      join Kimlik in db.Kimliks on applogin.kimlikid equals Kimlik.kimlikid
                      where Kimlik.tcno == tcno 
                      where applogin.sifre == sifre
                    
                      select new
                      {                        
                          applogin.apploginid,   
                          applogin.yetki,
                          Kimlik.tcno,
                          Kimlik.ad,
                          Kimlik.soyad,
                          Kimlik.dtarih,
                          Kimlik.sehir,
                          Kimlik.kangrubu
                      };
            if (sql.Count() == 0)
                return null;
            else return JsonConvert.SerializeObject(sql, Newtonsoft.Json.Formatting.Indented);
        }

        //yeni sifre oluşturma metodu update

        [WebMethod]
        public void setPassword(int inputId, string setpassword)
        {
            DataClassesLinqDataContext db = new DataClassesLinqDataContext();

            var sql = db.applogins.Where(w => w.apploginid == inputId  ).FirstOrDefault();
            sql.sifre = setpassword;
            db.SubmitChanges();                    

        }

        //acil mudahaleye yeni işlem  insert

        [WebMethod]
        public void setAcilMudahale(int inputpersonelid, int inputkisid, string inputnot)
        {
            DataClassesLinqDataContext db = new DataClassesLinqDataContext();

            AcilMudahale newmudahale = new AcilMudahale();
            newmudahale.personelid = inputpersonelid;
            newmudahale.kimlikid = inputkisid;
            newmudahale.tarih = DateTime.Now.ToString("dd.MM.yyyy");
            newmudahale.aciklama = inputnot;
            db.AcilMudahales.InsertOnSubmit(newmudahale);
            db.SubmitChanges();
        }


        //Acil Mudahale Metodu

        [WebMethod]
        public string getAcilMudahale(int inputId)
        {
            DataClassesLinqDataContext db = new DataClassesLinqDataContext();
            var sql = from AcilMudahale in db.AcilMudahales
                      join applogin in db.applogins on AcilMudahale.personelid equals applogin.apploginid
                      join Kimlik in db.Kimliks on applogin.kimlikid equals Kimlik.kimlikid
                      where AcilMudahale.kimlikid == inputId
                      select new
                      {
                          Kimlik.ad, // mudahale eden personelin adı
                          Kimlik.soyad,
                          applogin.yetki,          
                          AcilMudahale.tarih,
                          AcilMudahale.aciklama
                      };                                                            
            if (sql.Count() == 0)
                return null;
            else return JsonConvert.SerializeObject(sql, Newtonsoft.Json.Formatting.Indented);
        }

        //kisi metodu

        [WebMethod]
        public string getKisi(string inputQRTcno)
        {
            DataClassesLinqDataContext db = new DataClassesLinqDataContext();
            var sql = from Kimlik in db.Kimliks
                      where Kimlik.tcno == inputQRTcno 
                      select new
                      {
                          Kimlik.kimlikid,
                          Kimlik.tcno,
                          Kimlik.ad,
                          Kimlik.soyad,
                          Kimlik.dtarih,
                          Kimlik.sehir,
                          Kimlik.kangrubu
                      };

            if (sql.Count() == 0)
                return null;
            else return JsonConvert.SerializeObject(sql, Newtonsoft.Json.Formatting.Indented);
        }


        //iletisim metodu
        [WebMethod]
        public string getKisininİletisimBilgileri(int inputId)
        {
            DataClassesLinqDataContext db = new DataClassesLinqDataContext();

            var sql = from iletisim in db.iletisims
                      where iletisim.kimlikid == inputId                 
                      select new
                      {
                          iletisim.telefon,
                          iletisim.mail,
                          iletisim.yakinad,
                          iletisim.yakinsoyad,
                          iletisim.yakintel,
                          iletisim.yakinderece,
                          iletisim.adres
                          
                      };
            if (sql.Count() == 0)
                return null;
            else return JsonConvert.SerializeObject(sql, Newtonsoft.Json.Formatting.Indented);

        }

        //Kisinin ilac bilgileri

        [WebMethod]
        public string getIlacBilgisi(int inputId)
        {
            DataClassesLinqDataContext db = new DataClassesLinqDataContext();
            var sql = from ilac in db.ilacs
                      where ilac.kimlikid == inputId
                      select new
                      {
                          ilac.ilacad
                      };
            if (sql.Count() == 0)
                return null;
            else return JsonConvert.SerializeObject(sql, Newtonsoft.Json.Formatting.Indented);                    
        }

        //Kisinin hastalık bilgileri

        [WebMethod]
        public string getHastalikBilgisi(int inputId)
        {
            DataClassesLinqDataContext db = new DataClassesLinqDataContext();
            var sql = from hastalik in db.hastaliks                    
                      where hastalik.kimlikid == inputId
                      select new
                      {
                          hastalik.hastalikad
                      };
            if (sql.Count() == 0)
                return null;
            else return JsonConvert.SerializeObject(sql, Newtonsoft.Json.Formatting.Indented);
        }

        //Kisinin alerji bilgileri

        [WebMethod]
        public string getAlerjiBilgisi(int inputId)
        {
            DataClassesLinqDataContext db = new DataClassesLinqDataContext();
            var sql = from alerji in db.alerjis                     
                      where alerji.kimlikid == inputId
                      select new
                      {
                          alerji.alerjiad
                      };
            if (sql.Count() == 0)
                return null;
            else return JsonConvert.SerializeObject(sql, Newtonsoft.Json.Formatting.Indented);
        }

       

        //  getControlPerson metodu
        [WebMethod]
        public string getControlPerson(string inputTc, string inputAd, string inputSoyad, string inputDtarih, string inputSehir)
        {
            DataClassesLinqDataContext db = new DataClassesLinqDataContext();
            var sql = from applogin in db.applogins
                      join Kimlik in db.Kimliks on applogin.kimlikid equals Kimlik.kimlikid
                      where Kimlik.tcno == inputTc
                      where Kimlik.ad == inputAd
                      where Kimlik.soyad == inputSoyad
                      where Kimlik.dtarih == inputDtarih
                      where Kimlik.sehir == inputSehir
                      select new
                      {
                          applogin.apploginid

                      };
            if (sql.Count() == 0)
                return null;
            else return JsonConvert.SerializeObject(sql, Newtonsoft.Json.Formatting.Indented);
        }
        
    }
}
