 $(document).ready(function () {
            cc.init();
			 $("#main-body").show();
        });

        var cc = {
            init: function () {
                $("#forgotPassword").attr('href', 'javascript:void(0)');
                $("#createAccount").attr('href', 'javascript:void(0)');
				$("#next").text("Login");
				$("#createAccount").text("Register");
                cc.bindEvents();
                cc.initAppInsight();
				cc.showMessage()
            },
			
			showMessage: function () {
				var message = cc.getUrlParam("message");
				if (message) {				
					$("#logonIdentifier").before( "<p class='cc-success-message' style='color:green;'>" + message.replace(/%20/g, " ") + "</p>" );
					setTimeout(function() { $(".cc-success-message").hide(); },15000);
				}
			},
			
            initAppInsight: function () {
                var sdkInstance = "appInsightsSDK"; window[sdkInstance] = "appInsights"; var aiName = window[sdkInstance], aisdk = window[aiName] || function (e) { function n(e) { t[e] = function () { var n = arguments; t.queue.push(function () { t[e].apply(t, n) }) } } var t = { config: e }; t.initialize = !0; var i = document, a = window; setTimeout(function () { var n = i.createElement("script"); n.src = e.url || "https://az416426.vo.msecnd.net/scripts/b/ai.2.min.js", i.getElementsByTagName("script")[0].parentNode.appendChild(n) }); try { t.cookie = i.cookie } catch (e) { } t.queue = [], t.version = 2; for (var r = ["Event", "PageView", "Exception", "Trace", "DependencyData", "Metric", "PageViewPerformance"]; r.length;)n("track" + r.pop()); n("startTrackPage"), n("stopTrackPage"); var s = "Track" + r[0]; if (n("start" + s), n("stop" + s), n("addTelemetryInitializer"), n("setAuthenticatedUserContext"), n("clearAuthenticatedUserContext"), n("flush"), t.SeverityLevel = { Verbose: 0, Information: 1, Warning: 2, Error: 3, Critical: 4 }, !(!0 === e.disableExceptionTracking || e.extensionConfig && e.extensionConfig.ApplicationInsightsAnalytics && !0 === e.extensionConfig.ApplicationInsightsAnalytics.disableExceptionTracking)) { n("_" + (r = "onerror")); var o = a[r]; a[r] = function (e, n, i, a, s) { var c = o && o(e, n, i, a, s); return !0 !== c && t["_" + r]({ message: e, url: n, lineNumber: i, columnNumber: a, error: s }), c }, e.autoExceptionInstrumented = !0 } return t }(
                    {
                        instrumentationKey:  cc.getUrlParam("instrumentationKey")
                    }
                ); window[aiName] = aisdk, aisdk.queue && 0 === aisdk.queue.length && aisdk.trackPageView({ properties: { SubmissionId: cc.getUrlParam("submissionId") } });
            },

            bindEvents: function () {
                $("#logonIdentifier,#password").off().on('blur', function () {
                    var id = this.id;
					var value = this.value;
                    if (this.id === "logonIdentifier") {
                        id = "txtIdNumberB2cLoginPage";
                    }
                    else if (this.id === "password") {
					    value = "****";
                        id = "txtPasswordB2cLoginPage";
                    }
                    cc.saveTrackingLog('input', id, value);
                });
                $("#forgotPassword,#createAccount,#lnkHeader").off().on('click', function () {
                    var id = this.id;
					var href = "#";
                    if (this.id === "createAccount") {
                        id = "btnSignUpB2cLoginPage";
						href = decodeURIComponent(cc.getUrlParam('ccAppUrl')) + '/register';
                    }
                    else if (this.id === "forgotPassword") {
                        id = "btnForgotPasswordB2cLoginPage";
						href = decodeURIComponent(cc.getUrlParam('ccAppUrl')) + '/forgot';
                    }
					else if (this.id === "lnkHeader") {
                        id = "btnLogoB2cLoginPage";
						href = decodeURIComponent(cc.getUrlParam('ccAppUrl')) + '/home';
                    }
                    cc.saveTrackingLog('button', id, this.innerText);
					window.location.href = href;
                });
            },

            saveTrackingLog: function (elementType, elementName, elementValue) {
                appInsights.trackEvent({
                    name: elementType + 'Field',
                    properties: {
                        Type: elementType,
                        Name: elementName,
                        Value: elementValue,
                        SubmissionId: cc.getUrlParam("submissionId")
                    }
                });
            },

            getUrlParam: function (name) {
                var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
				if(results){                
					return results[1] || 0;
				}
				else{
					return "";
				}
            }
        }
