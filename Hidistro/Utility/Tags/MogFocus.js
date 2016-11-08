/*
 *  这是一个基于jQuery开发的focus插件
 *  版权所有归开发者蘑菇
 *  版本号： mogFocus v1.0
 *  感谢jquery学堂对作者的大力支持。
 *  如果您有建议或者在使用过程中发现BUG！请联系作者，提出您宝贵的意见！谢谢
 *  QQ:448214643
 */
(function($){
	$.fn.mogFocus = function(options){
		var opt =$.extend({},$.fn.mogFocus.defaults,options);
		return this.each(function(){
			var $this = $(this),
			$scroBox = $this.children().children().first(),
			$imgLen = $scroBox.children().length,
			$thisHeight = $this.height(),
			$imgWidth = $scroBox.find("img").attr("width"),
			$imghalf = Math.round($imgWidth/2),
			$indexImg = 0,
			$scroWidth = null,
			$arr = [],
			$randomWay = ['top','right','bottom','left','fadeTop','fadeRight','fadeBottom','fadeLeft'];
			!opt.single && (!!$scroBox.next("ul") &&$scroBox.next("ul").remove());
			if(opt.conversionImg){
				((opt.animationWay == 'fade' || opt.animationWay == 'randomImg') && $.browser.msie && parseInt($.browser.version)<=8) && pngFix();
				($.browser.msie && parseInt($.browser.version)==6) && pngFix();	
			}
			if((opt.scrollWidth == "100%") || (opt.scrollWidth == "auto")){
				$(window).bind("resize load",function(){
					var winWidth = $(window).width();
					$scroBox.parent().children().children().css("width",winWidth);
					var $imgWidth = $scroBox.children().width(); 
					if(opt.animationWay =='lrSliding'){
						$scroBox.parent().children("ul").css({"width":$imgWidth*$imgLen,"left":-($imgWidth*$indexImg)});
					}else{
						$scroBox.parent().children("ul").css("width",winWidth);
					}
				});
			}else{
				$scroBox.parent().children().children().css("width",opt.scrollWidth);
				$scroBox.parent().children().css("width",opt.scrollWidth*$imgLen);	
			}
			if(opt.animationWay =='fade' ||opt.animationWay=='randomImg'){
				$scroBox.parent().children("ul").children().css({"position":"absolute"});
				$scroBox.children().filter(":not(':first')").hide().parent().next("ul").children().filter(":not(':first')").hide();
			}
			$this.css("width",opt.scrollWidth).children().css("width",opt.scrollWidth);
			$arr.push("<ul class='imgBtn'>");
			$scroBox.children("li").each(function(i){
				switch(opt.btnStyle){
					case 'number':
						$arr.push("<li>"+(i+1)+"</li>");
						break;
					case 'noNumber':
						$arr.push("<li></li>");
						break;
					case 'thumbnail':
						var thum = $scroBox.children().children().eq(i).clone().find("img").attr({
							"width":opt.thumWidth,
							"height":opt.thumHeight
						}).parent().html();
						$arr.push("<li>"+thum+"</li>");
						break;
				}
			});
			$arr.push("</ul>");
			!(opt.btnStyle == "hidden") && $this.append($arr.join(''));
			!(opt.prevNextToggle == 'hide') && $scroBox.parent().prepend("<a href='#' class='prev'></a>").append("<a href='#' class='next'></a>");
			var $btn = $this.children(".imgBtn"),
			$prev = $this.find(".prev"),
			$next = $this.find(".next");
			$prev.css({"marginLeft":-($imghalf+opt.prevNextPos),"opacity":0});
			$next.css({"marginRight":-($imghalf+opt.prevNextPos),"opacity":0});
			opt.scrollWidth == "100%" || opt.scrollWidth == "auto" ? $btn.css(opt.thumCSS) : $btn.css("right",0);
			if(opt.animationWay == 'tbSliding'){
				var _liTwo = $scroBox.next("ul"),
					li_height = $scroBox.children().first().height(),
					_liTop = parseInt(_liTwo.css("top")),
					_arrTwo = [],
					_arrThr = [];
					reverseOrder($scroBox,_arrTwo);
					reverseOrder(_liTwo,_arrThr);
					_liTwo.children().css({"height":li_height-_liTop,"paddingTop":_liTop}).parent().css({"top":"auto","bottom":0});
					$scroBox.css("bottom",0);
			};
			if(opt.btnStyle=="thumbnail"){
				$btn.children().css(opt.thumSubsty).css({"width":opt.thumWidth,"height":opt.thumHeight});
				if($btn.children().length>opt.thumlen){
					$btn.removeAttr("class").wrap("<div class='imgBtn'></div>").parent().css(opt.thumCSS);
					$btn.before("<a href='#' class='thumPrev'></a>").after("<a href='#' class='thumNext'></a>");
					var _scrollLi = $btn.children(),
						_scroliLiMr = parseInt(_scrollLi.css("marginRight")),
						_scrollLiw = _scrollLi.outerWidth()+_scroliLiMr,
						_thumPrev = $(".thumPrev"),
						_thumNext = $(".thumNext"),
						_mobileLen = Math.ceil(opt.thumlen/2),
						_lastPage = Math.ceil(_scrollLi.length/_mobileLen),
						num = 0,
						loop = 0;
					$btn.parent().css("width",_scrollLiw*opt.thumlen+(_thumPrev.width()+parseInt(_thumPrev.css("marginRight")))*2);
					$btn.wrap("<div class='btnWarp'></div>");
					$this.find(".btnWarp").css({
						"float":"left",
						"position":"relative",
						"width":_scrollLiw*opt.thumlen,
						"height":_scrollLi.outerHeight()
					});
					$btn.css({
						"margin":0,
						"right":0,
						"position":"absolute",
						"left":0,
						"width":_scrollLiw*_scrollLi.length,
						"height":_scrollLi.outerHeight()
					});
					_thumPrev.css("visibility","hidden");
					_thumPrev.css("float","left").click(function(){
						if(num != 0){
							thumScroll("+=");	
							num-=1;
							loop -=_mobileLen;
						}
						return false;
					});
					_thumNext.css("float","right").click(function(){
						if(num != (_lastPage-1)){
							thumScroll("-=");
							num+=1;
							loop+=_mobileLen;
						}
						return false;
					});
					function thumScroll(operator){
						$btn.animate({"left":operator+_scrollLiw*_mobileLen},800,function(){
							num == (_lastPage-1)?_thumNext.css("visibility","hidden"):_thumNext.css("visibility","visible");
							num == 0?_thumPrev.css("visibility","hidden"):_thumPrev.css("visibility","visible");		
						});
					}
				}
			}
			
			switch(opt.btnStyle){
				case 'thumbnail': 
					$btn.children().eq(0).css(opt.thumSelected);
					break;
				case 'noNumber':
					$btn.children().eq(0).addClass("hover");
					break;
				case 'number':
				  	$btn.children().eq(0).css(opt.thumSelected).siblings().css(opt.thumSubsty);
				 	break;
			}
			$btn.children("li").each(function(){
				$(this).click(function(){
					if(!$scroBox.is(":animated") && !$scroBox.next("ul").is(":animated") && !$scroBox.children().is(":animated")){
						var index = $(this).index();
						$indexImg != index && scrolls(index);
						$indexImg = index;
						if(opt.btnStyle=="thumbnail"){
							if(index - loop ==(opt.thumlen-1)){thumScroll("-=");loop+=_mobileLen;num+=1;}
							if(index - loop ==0 && index!=0){thumScroll("+=");loop-=_mobileLen;num-=1;}
						}	
					}
				});	
			});
			var autoscroll;
			$this.hover(function(){
				opt.autoScroll && clearInterval(autoscroll);
				opt.prevNextToggle == "toggle" && prevNextIn();
			},function(){
				if(opt.autoScroll){
					autoscroll = setInterval(function(){
						$indexImg += 1;
						$indexImg == $imgLen && ($indexImg =0)
						scrolls($indexImg);
						
					},opt.time);		
				}
				opt.prevNextToggle == "toggle" && prevNextOut();
			}).trigger("mouseleave");
			$this.find(".prev").click(function(){
				if(!$scroBox.is(":animated") && !$scroBox.next("ul").is(":animated") && !$scroBox.children().is(":animated")){
					$indexImg -=1;
					$indexImg == -1 && ($indexImg = $imgLen-1);	
					scrolls($indexImg);
				}
				return false;
			}).hover(function(){
				$(this).fadeTo(300,0.6);	
			},function(){
				$(this).fadeTo(300,0.8);	
			});
			$this.find(".next").click(function(){
				if(!$scroBox.is(":animated") && !$scroBox.next("ul").is(":animated") && !$scroBox.children().is(":animated")){
					$indexImg +=1;
					$indexImg == $imgLen && ($indexImg =0);	
					scrolls($indexImg);
				}
				return false;
			}).hover(function(){
				$(this).fadeTo(300,0.6);
			},function(){
				$(this).fadeTo(300,0.8);
			});
			if(opt.loadswitch){
				var _loadImg = $this.find("img");
				$this.children().hide();
				$this.parent().css("background","url("+opt.loading+") no-repeat center center");
				$btn.hide();
				_loadImg.load(function(){
					$this.parent().css("background","none");
					opt.loadAnimation && loadAnim();
					$this.children("div").show();
				});
				if(!opt.loadAnimation){
					$btn.show();
					noneAnim();
				}		
			}else{
				opt.loadAnimation?loadAnim():noneAnim();	
			}
			function pngFix (){
				$scroBox.next("ul").find("img").each(function(i){
					var srcs = $(this).attr("src"),
						imgHs = $(this).attr("height"),
						imgWs = $(this).attr("width"),
						inters = srcs.substring(srcs.lastIndexOf(".")+1,srcs.length),
						_imgMl = parseInt($(this).css("marginLeft"));
					if(inters =="png"){
						$(this).wrap("<i></i>");
						$(this).parent().css({
							"display":"inline-block",
							"filter":'progid:DXImageTransform.Microsoft.AlphaImageLoader(src='+srcs+')',
							"height":imgHs,
							"width":imgWs,
							"marginLeft":_imgMl
						});
					$(this).remove();	
					}
				});		
			}
			function reverseOrder(element,arrName){
				element.children().each(function(){
					$(this).children().wrap("<li></li>");
					arrName.push($(this).html());	
				});
				element.html(arrName.reverse().join(''));
			}
			function prevNextIn(){
				$prev.animate({"marginLeft":-$imghalf,"opacity":0.8},opt.prevNextAnima);	
				$next.animate({"marginRight":-$imghalf,"opacity":0.8},opt.prevNextAnima);	
			}
			function prevNextOut(){
				$prev.animate({"marginLeft":-($imghalf+opt.prevNextPos),"opacity":0},opt.prevNextAnima);	
				$next.animate({"marginRight":-($imghalf+opt.prevNextPos),"opacity":0},opt.prevNextAnima);	
			}
			function scrosImg(action){
				  $scroBox.delay(opt.focusDelay).animate(action,opt.focusTime);
				  opt.single && $scroBox.next().delay(opt.focusDelayTwo).animate(action,opt.focusTwoTime);	
			}
			function randsImg(randsDir,emptys){
				var  _showImg = $scroBox.children(":visible"),
					_showImgTwo = $scroBox.next("ul").children(":visible")
				_showImg.css("z-index",10).delay(opt.focusDelayTwo).animate(randsDir,{
					duration: opt.randomsImgTime,
					easing: opt.randeasing,
					complete:function(){
						_showImg.hide().css(emptys)
					}
				});
				_showImgTwo.css("z-index",10).delay(opt.focusDelay).animate(randsDir,{
					duration: opt.randomsImgTime,
					easing: opt.randeasing,
					complete:function(){
						_showImgTwo.hide().css(emptys)
					}
				});
			}
		    function scrolls(i){
				opt.scrollWidth == "100%" || opt.scrollWidth == "auto" ? $scroWidth = $(window).width() : $scroWidth = opt.scrollWidth;	
				var animaWay = {
					'fade':	function(){
								$scroBox.children().eq(i).delay(opt.focusDelay).fadeIn(opt.fadeTime).siblings().fadeOut(opt.fadeTime);
								opt.single && $scroBox.next().children().eq(i).delay(opt.focusDelayTwo).fadeIn(opt.fadeTwoTime).siblings().fadeOut(opt.fadeTwoTime);
								},
					'lrSliding' : function(){
									scrosImg({"left":-($scroWidth*i)});
								},
					'tbSliding' : function(){
									scrosImg({"bottom":(-li_height*(i))});
								},
					'randomImg' : function(){
									var randImg = Math.floor(Math.random()*$randomWay.length),
									_imgHeight = $scroBox.children().height(),
									_imgWidth = $scroBox.children().width(),
									aniArr = [
										function(){randsImg({"top": -_imgHeight},{"top":0})},
										function(){randsImg({"left": _imgWidth},{"left":0})},
										function(){randsImg({"top": _imgHeight},{"top":0})},
										function(){randsImg({"left": -_imgWidth},{"left":0})},
										function(){randsImg({"top": -_imgHeight,"opacity":0},{"top":0,"opacity":1})},
										function(){randsImg({"left": _imgWidth,"opacity":0},{"left":0,"opacity":1})},
										function(){randsImg({"top": _imgHeight,"opacity":0},{"top":0,"opacity":1})},
										function(){randsImg({"left": -_imgWidth,"opacity":0},{"left":0,"opacity":1})}
									 ];
									 aniArr[randImg]();
									$scroBox.children().eq(i).css("z-index",1).show();
									$scroBox.next("ul").children().eq(i).css("z-index",1).delay(opt.focusDelay).fadeIn(600);
									
								  }
				}
				animaWay[opt.animationWay]();
				(opt.btnStyle == 'thumbnail' && $btn.children().length>opt.thumlen) && autoScroimg();
				switch(opt.btnStyle){ 
					case 'thumbnail':
					  $btn.children().eq(i).css(opt.thumSelected).siblings().css(opt.thumSubsty);
					  break;
					case 'noNumber': 
					  $btn.children().eq(i).addClass("hover").siblings().removeClass("hover");
					  break;
					case 'number':
					  $btn.children().eq(i).css(opt.thumSelected).siblings().css(opt.thumSubsty);
					  break;
				}
			}
			
				function autoScroimg(){
					if($btn.children().length>opt.thumlen){
						if($indexImg - loop ==(opt.thumlen-1)){
							thumScroll("-=");
							loop+=_mobileLen;
							num+=1;
							}else if($indexImg ==0){
								$btn.animate({'left':0},800);
								loop =0;
							}
						}
					}
			function loadAnim(){
				$btn.hide();
				$this.css({"top":Math.round($thisHeight/2),"height":0,"display":"block"}).delay(700).animate({
					"top":0,
					"height":$thisHeight
				},{duration: 1000, easing: "easeInOutQuart",complete:function(){
					$btn.fadeIn(600);
					opt.prevNextToggle == "show" && prevNextIn();
				}});	
			}
			function noneAnim(){
				$this.css("display","block");
				if(opt.prevNextToggle == 'show'){
					$prev.css({"marginLeft":-$imghalf,"opacity":0.8});
					$next.css({"marginRight":-$imghalf,"opacity":0.8});	
				}	
			}
		});
	}
	$.fn.mogFocus.defaults = {
		loadAnimation : true,  //效果初次加载是否有动画效果
		loadswitch : false,  //是否开启loading
		loading : "images/loading.gif",  //loading图片路径
		time : 3000,  //间隔时间
		scrollWidth : "100%",  //图片宽度，任意数值，设置为100%或者auto时，占满全屏
		autoScroll : true,   //是否自动滚动
		conversionImg : true, //此参数是用来兼容第二层滑动层IE的PNG图片，前提是您有gif图片，且图片名称相同，否则查找不到图片路径
		animationWay: 'lrSliding', //此参数提供fade(淡入淡出),lrSliding(左右滑动)，tbSliding(上下滑动)，随机展示(randomImg)
		prevNextPos : 70,  //上一页下一页初始位置调整
		prevNextAnima : {duration: 600, easing: "easeInOutBack"},  //按钮滑动出来的方式，支持缓动公式，如果直接设置数值，那么就没有缓动效果
		prevNextToggle : "hide",  //设置为toggle时鼠标放到滑动图上显示，离开影藏。设置show时一直显示，设置hide时影藏
		btnStyle: "number",  //参数有四个分别是number(数字),noNumber(非数字，任意图形),thumbnail(缩略图),hidden(影藏)
		thumWidth : 90,  //缩略图宽度
		thumHeight : 36,  //缩略图高度
		thumlen : 5,  //缩略图显示个数
		thumCSS : {"right":"50%","margin-right":-390},  //提供当全屏滑动时缩略图位置
		thumSubsty : {"border":"1px solid #ccc","padding":1,"background":"#fff"},  //缩略图样式
		thumSelected : {"border":"1px solid #6d6d6d","background":"#fff"},  //缩略图选中样式
		single: true,  //设置true则显示第二层，false则第二层影藏
		focusDelay: 0,  //焦点图延迟时间
		focusDelayTwo : 200,  //第二层焦点图延迟时间
		focusTime : {duration: 1300, easing: "easeInOutQuart"},  //左右焦点图时间，支持缓动公式，如果直接设置数值，那么就没有缓动效果
		focusTwoTime : {duration: 1300, easing: "easeInOutQuart"} , //左右第二层焦点图时间，支持缓动公式，如果直接设置数值，那么就没有缓动效果
		fadeTime : 400,  //该参数使用淡入淡出动画生效，控制淡入速度
		fadeTwoTime : 600, //该参数使用淡入淡出动画生效  控制第二层淡入速度
		randomsImgTime : 600,  //该参数在使用随机动画生效，控制滑动速度
		randeasing : 'easeInOutBack' //该参数在使用随机动画生效，控制easing滑动样式
	}
})(jQuery);



// t: current time, b: begInnIng value, c: change In value, d: duration
jQuery.easing['jswing'] = jQuery.easing['swing'];

jQuery.extend(jQuery.easing,
{
    def: 'easeOutQuad',
    swing: function (x, t, b, c, d) {
        //alert(jQuery.easing.default);
        return jQuery.easing[jQuery.easing.def](x, t, b, c, d);
    },
    easeInQuad: function (x, t, b, c, d) {
        return c * (t /= d) * t + b;
    },
    easeOutQuad: function (x, t, b, c, d) {
        return -c * (t /= d) * (t - 2) + b;
    },
    easeInOutQuad: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t + b;
        return -c / 2 * ((--t) * (t - 2) - 1) + b;
    },
    easeInCubic: function (x, t, b, c, d) {
        return c * (t /= d) * t * t + b;
    },
    easeOutCubic: function (x, t, b, c, d) {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    },
    easeInOutCubic: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t + 2) + b;
    },
    easeInQuart: function (x, t, b, c, d) {
        return c * (t /= d) * t * t * t + b;
    },
    easeOutQuart: function (x, t, b, c, d) {
        return -c * ((t = t / d - 1) * t * t * t - 1) + b;
    },
    easeInOutQuart: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
        return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
    },
    easeInQuint: function (x, t, b, c, d) {
        return c * (t /= d) * t * t * t * t + b;
    },
    easeOutQuint: function (x, t, b, c, d) {
        return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
    },
    easeInOutQuint: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
    },
    easeInSine: function (x, t, b, c, d) {
        return -c * Math.cos(t / d * (Math.PI / 2)) + c + b;
    },
    easeOutSine: function (x, t, b, c, d) {
        return c * Math.sin(t / d * (Math.PI / 2)) + b;
    },
    easeInOutSine: function (x, t, b, c, d) {
        return -c / 2 * (Math.cos(Math.PI * t / d) - 1) + b;
    },
    easeInExpo: function (x, t, b, c, d) {
        return (t == 0) ? b : c * Math.pow(2, 10 * (t / d - 1)) + b;
    },
    easeOutExpo: function (x, t, b, c, d) {
        return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b;
    },
    easeInOutExpo: function (x, t, b, c, d) {
        if (t == 0) return b;
        if (t == d) return b + c;
        if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
        return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
    },
    easeInCirc: function (x, t, b, c, d) {
        return -c * (Math.sqrt(1 - (t /= d) * t) - 1) + b;
    },
    easeOutCirc: function (x, t, b, c, d) {
        return c * Math.sqrt(1 - (t = t / d - 1) * t) + b;
    },
    easeInOutCirc: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return -c / 2 * (Math.sqrt(1 - t * t) - 1) + b;
        return c / 2 * (Math.sqrt(1 - (t -= 2) * t) + 1) + b;
    },
    easeInElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        return -(a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
    },
    easeOutElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        return a * Math.pow(2, -10 * t) * Math.sin((t * d - s) * (2 * Math.PI) / p) + c + b;
    },
    easeInOutElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d / 2) == 2) return b + c; if (!p) p = d * (.3 * 1.5);
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        if (t < 1) return -.5 * (a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
        return a * Math.pow(2, -10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
    },
    easeInBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        return c * (t /= d) * t * ((s + 1) * t - s) + b;
    },
    easeOutBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
    },
    easeInOutBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
        return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
    },
    easeInBounce: function (x, t, b, c, d) {
        return c - jQuery.easing.easeOutBounce(x, d - t, 0, c, d) + b;
    },
    easeOutBounce: function (x, t, b, c, d) {
        if ((t /= d) < (1 / 2.75)) {
            return c * (7.5625 * t * t) + b;
        } else if (t < (2 / 2.75)) {
            return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
        } else if (t < (2.5 / 2.75)) {
            return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
        } else {
            return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
        }
    },
    easeInOutBounce: function (x, t, b, c, d) {
        if (t < d / 2) return jQuery.easing.easeInBounce(x, t * 2, 0, c, d) * .5 + b;
        return jQuery.easing.easeOutBounce(x, t * 2 - d, 0, c, d) * .5 + c * .5 + b;
    }
});