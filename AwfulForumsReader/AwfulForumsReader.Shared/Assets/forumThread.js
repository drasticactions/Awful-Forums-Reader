window.SA || (SA = {});
SA.timg = new function(e, f, a) {
    var g = this,
        i = function(d, f) {
            var b = a(this).siblings("img"),
                c,
                h;
            if (b.attr("t_width"))
                a(this).removeClass("expanded"), b.attr({
                    width: b.attr("t_width"),
                    height: b.attr("t_height")
                }), b.removeAttr("t_width"), b.removeAttr("t_height");
            else {
                a(this).addClass("expanded");
                b.attr({
                    t_width: b.attr("width"),
                    t_height: b.attr("height")
                });
                var g = b.parents("blockquote");
                g.length || (g = b.parents(".postbody"));
                c = parseInt(b.attr("o_width"), 10);
                h = parseInt(b.attr("o_height"), 10);
                g = Math.min(900, g.width());
                if (f && c > g) {
                    var i = b.position(),
                        g = (g - 3 * i.left) / c;
                    b.attr("width", c * g);
                    b.attr("height", h * g);
                } else b.removeAttr("width"), b.removeAttr("height");
                g = a.browser.webkit || a.browser.safari ? "body" : "html";
                h = a(g).scrollTop();
                c = b.offset().top;
                b = c + b.height();
                b - h > a(e).height() && (h = b - a(e).height());
                c < h && (h = c);
                h != a(g).scrollTop() && (a.browser.msie && 7 > parseInt(a.browser.version, 10) ? a(g).scrollTop(h) : a(g).animate({
                    scrollTop: h
                }, 150));
            }
            return !1;
        },
        k = function() {
            var d = a(this);
            if (d.hasClass("loading")) {
                d.removeClass("loading");
                var e = d[0].naturalWidth || d.width(),
                    b = d[0].naturalHeight || d.height();
                if (200 > b && 500 >= e || 170 > e) d.removeClass("timg");
                else {
                    d.addClass("complete");
                    d.attr({
                        o_width: e,
                        o_height: b
                    });
                    var e = e + "x" + b,
                        b = 1,
                        c = d[0].naturalWidth || d.width(),
                        h = d[0].naturalHeight || d.height();
                    170 < c && (b = 170 / c);
                    200 < h * b && (b = 200 / h);
                    d.attr({
                        width: c * b,
                        height: h * b
                    });
                    var b = a('<span class="timg_container"></span>'),
                        f = a('<div class="note"></div>');
                    f.text(e);
                    f.attr("title", "Click to toggle");
                    b.append(f);
                    d.before(b);
                    b.prepend(d);
                    f.click(i);
                    b.click(function(b) {
                        if (1 === b.which || a.browser.msie && 9 > parseInt(a.browser.version, 10) && 0 === b.which) return i.call(f, b, !0), !1;
                    });
                }
                d.trigger("timg.loaded");
            }
        };
    g.scan = function(d) {
        a(d).find("img.timg").each(function(d, b) {
            b = a(b);
            b.hasClass("complete") || (b.addClass("loading"), b[0].complete || null !== b[0].naturalWidth && 0 < b[0].naturalWidth ? k.call(b) : b.load(k));
        });
    };
    a(f).ready(function() {
        g.scan("body");
    });
    a(e).load(function() {
        var d = a("img.timg.loading");
        d.length && d.each(function(a, b) {
            k.call(b);
        });
    });
}(window, document, jQuery);