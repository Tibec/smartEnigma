

//////  FUNCTION //////////
/*function centrerElement(element)
{
	var largeur_fenetre = $(window).width();
	var hauteur_fenetre = $(window).height();
}

*/
/////  MAIN /////////////


//var c = document.getElementById('canv');
//alert(c.height + ' ' + c.width);
//c.height = 200;
//c.width = 200;
//alert(c.height + ' ' + c.width);

var c = document.getElementById("myCanvas");
/*
var ratio = window.devicePixelRatio || 1;
var w = screen.width * ratio;
var h = screen.height * ratio;
*/

//var w = Math.max(document.documentElement.clientWidth, window.innerWidth || 0);
//var h = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);

var w = window.innerWidth;
var h = window.innerHeight;

c.height=h;
c.width=w;

var ctx = c.getContext("2d");
ctx.rect(20, 20, 150, 100);
ctx.stroke();

//document.getElementById('canv').width = screen.width;
//document.getElementById('canv').height = screen.height;


//var largeur_fenetre = $(window).width();
//var hauteur_fenetre = $(window).height();


