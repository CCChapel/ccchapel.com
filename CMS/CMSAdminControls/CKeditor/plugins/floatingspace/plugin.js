﻿(function () { function r(e) { var n = e == "left" ? "pageXOffset" : "pageYOffset", r = e == "left" ? "scrollLeft" : "scrollTop"; return n in t.$ ? t.$[n] : CKEDITOR.document.$.documentElement[r] } function i(i) { var s = i.config, o = i.fire("uiSpace", { space: "top", html: "" }).html, u = function () { function b(e, t, r) { a.setStyle(t, n(r)); a.setStyle("position", e) } function w(t) { var n = o.getDocumentPosition(); switch (t) { case "top": b("absolute", "top", n.y - h - m); break; case "pin": b("fixed", "top", y + d); break; case "bottom": b("absolute", "top", n.y + (l.height || l.bottom - l.top) + m); break } e = t } var e, o, f, l, c, h, p, d = $cmsj("#CMSHeaderDiv").outerHeight(), v = s.floatSpaceDockedOffsetX || 0, m = s.floatSpaceDockedOffsetY || 0, g = s.floatSpacePinnedOffsetX || 0, y = s.floatSpacePinnedOffsetY || 0; return function (s) { if (!(o = i.editable())) return; s && s.name == "focus" && a.show(); a.removeStyle("left"); a.removeStyle("right"); f = a.getClientRect(); l = o.getClientRect(); c = t.getViewPaneSize(); h = f.height; p = r("left"); if (!e) { e = "pin"; w("pin"); u(s); return } if (h + m <= l.top - d) w("top"); else if (h + m > c.height - l.bottom - d) w("pin"); else w("bottom"); var y = c.width / 2, b = l.left > 0 && l.right < c.width && l.width > f.width ? i.config.contentsLangDirection == "rtl" ? "right" : "left" : y - l.left > l.right - y ? "left" : "right", E; if (f.width > c.width) { b = "left"; E = 0 } else { if (b == "left") { if (l.left > 0) E = l.left; else E = 0 } else { if (l.right < c.width) E = c.width - l.right; else E = 0 } if (E + f.width > c.width) { b = b == "left" ? "right" : "left"; E = 0 } } var S = e == "pin" ? 0 : b == "left" ? p : -p; a.setStyle(b, n((e == "pin" ? g : v) + E + S)) } }(); if (o) { var a = CKEDITOR.document.getBody().append(CKEDITOR.dom.element.createFromHtml(e.output({ content: o, id: i.id, langDir: i.lang.dir, langCode: i.langCode, name: i.name, style: "display:none;z-index:" + (s.baseFloatZIndex - 1), topId: i.ui.spaceId("top"), voiceLabel: i.lang.editorPanel + ", " + i.name }))), f = CKEDITOR.tools.eventsBuffer(500, u), l = CKEDITOR.tools.eventsBuffer(100, u); a.unselectable(); a.on("mousedown", function (e) { e = e.data; if (!e.getTarget().hasAscendant("a", 1)) e.preventDefault() }); i.on("focus", function (e) { u(e); i.on("change", f.input); t.on("scroll", l.input); t.on("resize", l.input) }); i.on("blur", function () { a.hide(); i.removeListener("change", f.input); t.removeListener("scroll", l.input); t.removeListener("resize", l.input) }); i.on("destroy", function () { t.removeListener("scroll", l.input); t.removeListener("resize", l.input); a.clearCustomData(); a.remove() }); if (i.focusManager.hasFocus) a.show(); i.focusManager.add(a, 1) } } var e = CKEDITOR.addTemplate("floatcontainer", "<div" + ' id="cke_{name}"' + ' class="cke {id} cke_reset_all cke_chrome cke_editor_{name} cke_float cke_{langDir} ' + CKEDITOR.env.cssClass + '"' + ' dir="{langDir}"' + ' title="' + (CKEDITOR.env.gecko ? " " : "") + '"' + ' lang="{langCode}"' + ' role="application"' + ' style="{style}"' + ' aria-labelledby="cke_{name}_arialbl"' + ">" + '<span id="cke_{name}_arialbl" class="cke_voice_label">{voiceLabel}</span>' + '<div class="cke_inner">' + '<div id="{topId}" class="cke_top" role="presentation">{content}</div>' + "</div>" + "</div>"), t = CKEDITOR.document.getWindow(), n = CKEDITOR.tools.cssLength; CKEDITOR.plugins.add("floatingspace", { init: function (e) { e.on("loaded", function () { i(this) }, null, null, 20) } }) })()