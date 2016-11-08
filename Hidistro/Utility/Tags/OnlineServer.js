function showKefu(val) {
    val.style.display = 'none';
    document.getElementById("qq_right").style.left = 0 + "px";
}
function closekf() {
    document.getElementById("e").style.display = '';
    document.getElementById("qq_right").style.left = -146 + "px";
}
//滚动代码开始
function qqshow() {
    if (document.body.offsetWidth > 900) {
        if (document.getElementById("qq_right") != null) {
            var scrollTop = window.pageYOffset ? window.pageYOffset : document.documentElement.scrollTop;
            document.getElementById("qq_right").style.top = (scrollTop + 30) + "px";
        }
        else { return false }
    }
    else {
        document.getElementById("qq_right").style.display = "none";
    }
}
function showqq() {
    setTimeout("qqshow();", 10);
}

function fullScreen() {
    this.moveTo(0, 0);
    this.outerWidth = screen.availWidth;
    this.outerHeight = screen.availHeight;
}

window.onscroll = showqq;
window.onresize = qqshow;
window.onload = qqshow;
window.maximize = fullScreen;