$(document).ready(function () {
    $(document).on("click", "#KategoriDelete", function () {
        debugger;
        var gelenID = $(this).attr("data-id");
        var silTr = $(this).closest("tr");
        $.ajax({
            url: '/Kategori/Sil/' + gelenID,
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    $.notify(response.Message, "success");
                    silTr.fadeOut(300, function () {
                        silTr.remove();
                    })
                }
                else {
                    $.notify(response.Message, "error");
                }
                
            }
        })

    })
})

function KategoriEkle() {
    Kategori = new Object();
    Kategori.KategoriAdi = $("#kategoriAdi").val();
    Kategori.Url = $("#kategoriUrl").val();
    Kategori.AktifMi = $("#kategoriAktifMi").is(":checked");
    Kategori.ParentID = $("#ParentID").val();

    $.ajax({
        url: "/Kategori/Ekle/",
        data: Kategori,
        type: "POST",
        success: function (response)
        {
            if (response.Success)
            {
                bootbox.alert(response.Message, function () {
                    location.href = "/Kategori";
                });
            }
            else
            {
                bootbox.alert(response.Message, function () {
                    //geridöndüğünde bir şey yapması isteniyorsa buraya yazılır
                });
            }

        }
    })

}



function KategoriDuzenle() {

    Kategori = new Object();
    Kategori.KategoriAdi = $("#kategoriAdi").val();
    Kategori.Url = $("#kategoriUrl").val();
    Kategori.AktifMi = $("#kategoriAktifMi").is(":checked");
    Kategori.ParentID = $("#ParentID").val();
    Kategori.Id = $("#Id").val();

    $.ajax({
        url: "/Kategori/Duzenle/",
        data: Kategori,
        type: "POST",
        success: function (response) {
            if (response.Success) {
                bootbox.alert(response.Message, function () {
                    location.href = "/Kategori";
                });
            }
            else {
                bootbox.alert(response.Message, function () {
                    //geridöndüğünde bir şey yapması isteniyorsa buraya yazılır
                });
            }

        }
    })
}