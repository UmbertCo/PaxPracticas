function share(url,w,h){
	var left = (screen.width/2)-(w/2);
  	var top = (screen.height/2)-(h/2);
	window.open('https://www.facebook.com/sharer/sharer.aspx?u='+url, 'facebook-share-dialog', 'width='+w+', height='+h+', top='+top+', left='+left); 
	return false;
}

$(window).load(function() {
	$('.flexslider').flexslider({
		animation:"slide",
		controlNav: false,
		directionNav: false
	});

$(".accordion").accordion({
        canToggle: true
    });

$(".accordion-2").accordion({
        canToggle: true
    });

/* ==========================================================================
   	PASOS CSD
========================================================================== */
		
$(".botonPaso").click(function(e) {
    
	alt = $(this).attr("alt");
	
	$("#pasos-csd > div").hide();
	
	$("#pasos-csd > div").each(function(index, element) {
        id = $(this).attr("id");
		
		if(alt == id){
			$(this).fadeIn();
		}
    });
	
	return false;
	
});

/* ==========================================================================
   	Volver arriba
========================================================================== */

$(".toTop").click(function () {

   $("html, body").animate({scrollTop: 398}, 1000);

});

/* ==========================================================================
   	Validaciones
========================================================================== */

$("#valida").validate({
		messages: {
		},
		submitHandler: function(form){
		$.ajax({
			type: "POST",
			url: "http://projects.xpectare.com/pax/envio.aspx",
			data: $(form).serializeArray(),
			success: function(){
			$('#exito-suscribete').show('slow','linear');
				setTimeout(function(){
			$('#exito-suscribete').hide('slow','linear');
			$(form).each(function(){
				this.reset();
			});
		}, 3000);
		console.log("enviado");
		},
		error: function(){
		alert("Hubo un error");
		}
		});
		}
	});
	
$("#valida-interna").validate({
		messages: {
		},
		submitHandler: function(form){
		$.ajax({
			type: "POST",
			url: "http://projects.xpectare.com/pax/envio.aspx",
			data: $(form).serializeArray(),
			success: function(){
			$('#exito-interna').show('slow','linear');
				setTimeout(function(){
			$('#exito-interna').hide('slow','linear');
			$(form).each(function(){
				this.reset();
			});
		}, 5000);
		console.log("enviado");
		},
		error: function(){
		alert("Hubo un error");
		}
		});
		}
	});	
	
	
$("#contactoForm").validate({
		messages: {
		},
		submitHandler: function(form){
		$.ajax({
			type: "POST",
			url: "http://projects.xpectare.com/pax/envio.aspx",
			data: $(form).serializeArray(),
			success: function(){
			$('#exito').show('slow','linear');
				setTimeout(function(){
			$('#exito').hide('slow','linear');
			$(form).each(function(){
				this.reset();
			});
		}, 5000);
		console.log("enviado");
		},
		error: function(){
		alert("Hubo un error");
		}
		});
		}
	});		

/* ==========================================================================
   	Fancybox (Lightbox)
========================================================================== */

$(".fancybox").fancybox({
	type: 'iframe',
	autoSize : false,
	beforeLoad : function() {         
            this.width  = parseInt(this.element.data('fancybox-width'));  
            this.height = parseInt(this.element.data('fancybox-height'));
        },
	helpers : {
        overlay : {
            css : {
                'background' : 'rgba(58, 42, 45, 0.95)'
            }
        }
    }
});

/* ==========================================================================
   	Formulario de Registro
========================================================================== */

$('#dt').change(function(){
        if (this.checked) {
            $('.datos-fiscales').fadeIn('slow');
        }
        else {
            $('.datos-fiscales').fadeOut('slow');
        }                   
    });

/* ==========================================================================
   	Chat en Línea
========================================================================== */

$( ".chatenlinea" ).click(function() {
	$zopim.livechat.window.show();
});

});