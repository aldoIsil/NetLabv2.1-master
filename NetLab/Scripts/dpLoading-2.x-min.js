if (dpUI === undefined) var dpUI = { data: {}, helper: {}, options: {}, versions: {} };
dpUI.versions.loading = "2.2.0";
dpUI.loading = { start: function (e, t, n, r) { if (e === undefined) e = ""; if (t === undefined || t === true) t = "<i class='fa fa-spinner fa-spin'></i>"; if ($(".dpui-loading").size() > 0) dpUI.loading.stop(); var i = "<div class='dpui-loading'><div class='dpui-loading-inner'>"; if (t) i += "<div class='dpui-loading-gif'>" + t + "</div>"; i += "<div class='dpui-loading-text'>" + e + "</div></div></div>"; $("body").addClass("noscroll").append(i); if (n) { if (r === undefined) r = function () { }; $(".dpui-loading")[0].timeoutid = window.setTimeout(function () { $(".dpui-loading").remove(); $("body").removeClass("noscroll"); r() }, n) } }, stop: function () { if ($(".dpui-loading")[0].timeoutid) window.clearTimeout($(".dpui-loading")[0].timeoutid); $(".dpui-loading").remove(); $("body").removeClass("noscroll") } }; (function (e) { e.fn.dpLoading = function (e, t, n, r) { e = e.toLowerCase(); if (e == "start") dpUI.loading.start(t, n, r); else dpUI.loading.stop() } })(jQuery);