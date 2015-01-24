$(document).ready(function () {
    $('#btnRjesenje').click(function () {
        var rj = $('#rjesenjeBox').val();
        var id = $('#idDioIgre').val();
        $.ajax({
            type: "POST",
            data: {
                id: id,
                tekst: rj
            },
            url: "/Pozicija/RjesiProblem",
            success: function (result) {
                debugger;
                document.getElementById('uspjesnostRjesenja').innerHTML = '<p>' + result.rezultat + '</p>';
                document.getElementById('komentarRjesenja').innerHTML = '<p>' + result.komentar + '</p>';
                document.getElementById('poteziRjesenja').innerHTML = '<p>' + result.ispisPoteza + '</p>';
                document.getElementById('rjesenjeBox').innerHTML = '';
            },
            error: function (result) {
                alert("Neuspjesna promjena.");
            }
        });
    });
});