namespace HaberSistemi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Olustur : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Haber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Baslik = c.String(nullable: false, maxLength: 255),
                        KisaAciklama = c.String(),
                        Aciklama = c.String(),
                        Okunma = c.Int(nullable: false),
                        Resim = c.String(maxLength: 255),
                        EklenmeTarihi = c.DateTime(nullable: false),
                        AktifMi = c.Boolean(nullable: false),
                        Kategori_Id = c.Int(),
                        Kullanici_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kategori", t => t.Kategori_Id)
                .ForeignKey("dbo.Kullanici", t => t.Kullanici_Id)
                .Index(t => t.Kategori_Id)
                .Index(t => t.Kullanici_Id);
            
            CreateTable(
                "dbo.Kategori",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KategoriAdi = c.String(nullable: false, maxLength: 150),
                        ParentID = c.Int(nullable: false),
                        URL = c.String(maxLength: 150),
                        EklenmeTarihi = c.DateTime(nullable: false),
                        AktifMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Kullanici",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdSoyad = c.String(maxLength: 150),
                        Email = c.String(nullable: false),
                        Sifre = c.String(nullable: false, maxLength: 16),
                        EklenmeTarihi = c.DateTime(nullable: false),
                        AktifMi = c.Boolean(nullable: false),
                        Rol_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rol", t => t.Rol_Id)
                .Index(t => t.Rol_Id);
            
            CreateTable(
                "dbo.Rol",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RolAdi = c.String(maxLength: 150),
                        EklenmeTarihi = c.DateTime(nullable: false),
                        AktifMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Resim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResimUrl = c.String(),
                        EklenmeTarihi = c.DateTime(nullable: false),
                        AktifMi = c.Boolean(nullable: false),
                        Haber_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Haber", t => t.Haber_Id)
                .Index(t => t.Haber_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resim", "Haber_Id", "dbo.Haber");
            DropForeignKey("dbo.Haber", "Kullanici_Id", "dbo.Kullanici");
            DropForeignKey("dbo.Kullanici", "Rol_Id", "dbo.Rol");
            DropForeignKey("dbo.Haber", "Kategori_Id", "dbo.Kategori");
            DropIndex("dbo.Resim", new[] { "Haber_Id" });
            DropIndex("dbo.Haber", new[] { "Kullanici_Id" });
            DropIndex("dbo.Kullanici", new[] { "Rol_Id" });
            DropIndex("dbo.Haber", new[] { "Kategori_Id" });
            DropTable("dbo.Resim");
            DropTable("dbo.Rol");
            DropTable("dbo.Kullanici");
            DropTable("dbo.Kategori");
            DropTable("dbo.Haber");
        }
    }
}
