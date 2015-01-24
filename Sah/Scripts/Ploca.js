

$(document).ready(function () {
    $('#btnSljedeci').click(function () {
        var brojac =parseInt($('#idBrojacPotez').val());
        var potezi = $('#idPotezi').val();
        var figure = $('#idFigure').val();
        var pojedinacniPotezi = potezi.split(',');
        var trenutniPotez = pojedinacniPotezi[brojac];
        var potezKomentar = trenutniPotez.split('/');
        //ovo je potez
        var potez=potezKomentar[0];
        //ovo je komentar uz potez
        var komentar = potezKomentar[1];
        document.getElementById('komentarPotez').innerHTML = '<p></p>';
        document.getElementById('komentarPotez').innerHTML = '<p>'+komentar+'</p>';
        var pocLokacija = potez.substring(0, 2);
        var odredLokacija = potez.substring(3);
        //postavljanje brojaca na vrijednost za jedan vecu
        //tako brojimo poteze
        brojac = brojac + 1;
        $('#idBrojacPotez').val(brojac.toString());
        var pocFigura = document.getElementById(pocLokacija).innerHTML;
        $('#'+pocLokacija).empty();
        //document.getElementById(pocLokacija).innerHTML = '<p></p>';
        var odredFigura = document.getElementById(odredLokacija).innerHTML;
        if (document.getElementById(odredLokacija).innerHTML.length != 0)
        {
            figure += odredLokacija + '&' + brojac.toString() + '&' + document.getElementById(odredLokacija).innerHTML + '&&';
            $('#idFigure').val(figure);
        }
        document.getElementById(odredLokacija).innerHTML = pocFigura;
        
    });
});

$(document).ready(function () {
    $('#btnProsli').click(function () {
        var brojac = parseInt($('#idBrojacPotez').val());
        brojac = brojac - 1;
        var potezi = $('#idPotezi').val();
        var figure = $('#idFigure').val();
        var pojedinacniPotezi = potezi.split(',');
        var trenutniPotez = pojedinacniPotezi[brojac];
        var potezKomentar = trenutniPotez.split('/');
        if (brojac == 0) {
            $('#komentarPotez').empty();
            //document.getElementById('komentarPotez').innerHTML = '<p></p>';
        }
        if (brojac > 0) {
            var prosliPotez = pojedinacniPotezi[brojac - 1];
            var potezKom = prosliPotez.split('/');
            var kom = potezKom[1];
            $('#komentarPotez').empty();
            //document.getElementById('komentarPotez').innerHTML = '<p></p>';
            if ((brojac) != 0) {
                document.getElementById('komentarPotez').innerHTML = '<p>' + kom + '</p>';
            }
        }
        //pojedene figure
        var pojedineFigure = figure.split('&&');
        var inner = '';
        for (var i = 0; i < pojedineFigure.length; i++)
        {
            var podaci = pojedineFigure[i].split('&');
            var trBrojac = parseInt(podaci[1]);
            if (trBrojac == (brojac + 1))
            {
                var inner = podaci[2];
            }
        }
        //ovo je potez
        var potez = potezKomentar[0];
        var odredLokacija = potez.substring(0, 2);
        var pocLokacija = potez.substring(3);
        //postavljanje brojaca na vrijednost za jedan manju
        //tako brojimo poteze
        $('#idBrojacPotez').val(brojac.toString());
        var figura = document.getElementById(pocLokacija).innerHTML;
        //document.getElementById(pocLokacija).innerHTML = '<p></p>';
        $('#' + pocLokacija).empty();
        document.getElementById(odredLokacija).innerHTML = figura;
        if (inner.length != 0)
        {
            document.getElementById(pocLokacija).innerHTML = inner;
        }
    });
});