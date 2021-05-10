using SoruCevap.Models;
using SoruCevap.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SoruCevap.Controllers
{
    public class ServiceController : ApiController
    {
        SonucModel sonuc = new SonucModel();

        Database1Entities db = new Database1Entities();


        #region Uye API

        [HttpGet]
        [Route("api/uyebyid/{id}")]
        public UyeModel uyeById(string id)
        {
            UyeModel uye = db.Uye.Where(s => s.userId == id).Select(x => new UyeModel()
            {
                ad = x.ad,
                soyad = x.soyad,
                userId = x.userId,
                email = x.email,
                rol = x.rol,
                sifre = x.sifre
            }).SingleOrDefault();

            return uye;
        }

        [HttpPost]
        [Route("api/uyeekle")]
        public SonucModel uyeEkle(UyeModel uye)
        {
            if(db.Uye.Count(s=> s.userId == uye.userId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt zaten var";
                return sonuc;
            }
            Uye newUye = new Uye();
            newUye.userId = Guid.NewGuid().ToString();
            newUye.ad = uye.ad;
            newUye.soyad = uye.soyad;
            newUye.sifre = uye.sifre;
            newUye.email = uye.email;
            newUye.rol = uye.rol;
            db.Uye.Add(newUye);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Üye oluşturuldu";
            return sonuc;
        }

        [HttpPut]
        [Route("api/uye")]
        public SonucModel uyeDuzenle(UyeModel uye)
        {
            Uye kayit = db.Uye.Where(s => s.userId == uye.userId).SingleOrDefault();
            
            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            kayit.ad = uye.ad;
            kayit.soyad = uye.soyad;
            kayit.email = uye.email;
            kayit.sifre = uye.sifre;
            kayit.rol = uye.rol;
            db.Uye.Add(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Üye Güncellendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/uyebyid/{id}")]
        public SonucModel uyeSil(string id)
        {
            Uye kayit = db.Uye.Where(s => s.userId == id).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Silinecek üye bulunamadı";
                return sonuc;
            }

            if(db.Soru.Count(s=>s.uyeId== id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Sorusu olan üyeler silinemez";
                return sonuc;
            }

            db.Uye.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Üye kaldırıldı";
            return sonuc;
        }


        #endregion

        #region Soru API

        [HttpGet]
        [Route("api/soru")]
        public List<SoruModel> soruListele()
        {
            List<SoruModel> liste = db.Soru.Select(x => new SoruModel
            {
                baslik = x.baslik,
                icerik = x.icerik,
                Id = x.Id,
                katId = x.katId,
                tarih = x.tarih,
                uyeId = x.uyeId
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/sorubyid/{id}")]
        public SoruModel soruById(int id)
        {
            SoruModel kayit = db.Soru.Where(s => s.Id == id).Select(x => new SoruModel()
            {
                baslik = x.baslik,
                Id = x.Id,
                icerik = x.icerik,
                katId = x.katId,
                tarih = x.tarih,
                uyeId = x.uyeId
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/soru")]
        public SonucModel soruEkle(SoruModel model)
        {
            if(db.Soru.Count(s=> s.Id == model.Id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu ID de soru bulunmaktadır.";
                return sonuc;
            }

            Soru newSoru = new Soru();
            newSoru.baslik = model.baslik;
            newSoru.icerik = model.icerik;
            newSoru.tarih = model.tarih;
            newSoru.katId = model.katId;
            newSoru.uyeId = model.uyeId;
            db.Soru.Add(newSoru);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Soru oluşturuldu";
            return sonuc;
        }

        [HttpPut]
        [Route("api/soru")]
        public SonucModel soruDuzenle(SoruModel model)
        {
            Soru kayit = db.Soru.Where(s => s.Id == model.Id).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            kayit.icerik = model.icerik;
            kayit.baslik = model.baslik;
            kayit.tarih = model.tarih;
            kayit.katId = model.katId;
            kayit.uyeId = model.uyeId;
            db.Soru.Add(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kayıt güncellendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/sorubyid/{id}")]
        public SonucModel soruSil(int id)
        {
            Soru kayit = db.Soru.Where(s => s.Id == id).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            if(db.Cevap.Count(s=>s.soruId == id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde yanıt olan sorular silinemez";
                return sonuc;
            }

            db.Soru.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = false;
            sonuc.mesaj = "Soru kaldırıldı";
            return sonuc;
        }

        #endregion

        #region Kategori API

        [HttpGet]
        [Route("api/kategori")]
        public List<KategoriModel> kategoriListele()
        {
            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel()
            {
                katAdi = x.katAdi,
                katId = x.katId
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/kategoribyid/{id}")]
        public KategoriModel kategoriById(int id)
        {
            KategoriModel kayit = db.Kategori.Where(s => s.katId == id).Select(x => new KategoriModel()
            {
                katId = x.katId,
                katAdi = x.katAdi
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/kategori")]
        public SonucModel kategoriEkle(KategoriModel model) 
        {
            if(db.Kategori.Count(s=> s.katAdi == model.katAdi)>0) 
            {
                sonuc.islem = false;
                sonuc.mesaj = "Verilen kategori türü kayıtlıdır";
                return sonuc;
            }

            Kategori yeni = new Kategori();
            yeni.katAdi = model.katAdi;
            db.Kategori.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori oluşturuldu";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kategori")]
        public SonucModel kategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(s => s.katId == model.katId).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori bulunamadı.";
                return sonuc;
            }

            kayit.katAdi = model.katAdi;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori güncellendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategoribyid/{id}")]
        public SonucModel kategoriSil(int id)
        {
            Kategori kayit = db.Kategori.Where(s => s.katId == id).SingleOrDefault();
            
            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori bulunamadı";
                return sonuc;
            }

            if(db.Soru.Count(s=>s.katId == id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde soru bulunan kategori silinemez";
                return sonuc;
            }

            db.Kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori kaldırıldı";
            return sonuc;
        }
        #endregion

        #region Cevap API

        [HttpGet]
        [Route("api/cevapbyid/{soruId}")]
        public List<CevapModel> cevapListeleBySoruId(int soruId)
        {
            List<CevapModel> liste = db.Cevap.Where(s => s.soruId == soruId).Select(x=> new CevapModel()
            {
                icerik = x.icerik,
                soruId = x.soruId,
                Id = x.Id,
                uyeId =x.uyeId
            }  ).ToList();

            return liste;
        }

        [HttpPost]
        [Route("api/cevap")]
        public SonucModel cevapEkle(CevapModel model)
        {
            Cevap yeni = new Cevap();
            yeni.icerik = model.icerik;
            yeni.soruId = model.soruId;
            yeni.uyeId = model.uyeId;
            db.Cevap.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Cevap oluşturuldu";
            return sonuc;
            
        }

        [HttpPut]
        [Route("api/cevap")]
        public SonucModel cevapDuzenle(CevapModel model)
        {
            Cevap kayit = db.Cevap.Where(s => s.Id == model.Id).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            kayit.icerik = model.icerik;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Cevap güncellendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/cevapbyid/{id}")]
        public SonucModel cevapSil(int id)
        {
            Cevap kayit = db.Cevap.Where(s=>s.Id == id).SingleOrDefault();

            if(kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Silinecek kayıt bulunamadı";
                return sonuc;
            }
            db.Cevap.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Cevap kaldırıldı";
            return sonuc;
        }
        #endregion
    }
}
