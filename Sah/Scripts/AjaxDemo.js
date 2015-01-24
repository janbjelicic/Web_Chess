$(document).ready(function () {                             
    $('#btnDodajKomentar').click(function () {              
        var komentar = $('#commentBox').val();
        var id = $('#idDioIgre').val();
        $.ajax({
            type: "POST",
            data: {
                id: id,
                tekst: komentar                
            },
            url: "/Pozicija/DodajKomentar",
            success: function (result) {
                debugger;
                $("ul.komentari").append("<li>" + result.sadrzaj+"<br/>" +result.korisnickoIme+","+  result.datum + "</li>");
            },
            error: function (result) {
                alert("Neuspjesna promjena.");
            }
        });
    });
});