<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JsTimer.aspx.cs" Inherits="JQueryUI.JsTimer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JS Timer</title>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.2/jquery-ui.min.js"
        type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Time：<span id="echo"></span> ...<br />
        <span id="echo1"></span>
    </div>
    </form>
    <script type="text/javascript">
        Scheduler = function (interval, onTimeCallback) {
            this._interval = interval;
            this._onTimeCallback = onTimeCallback;
            this.startTimer = function () {
                var self = this;
                function updateTimer() {
                    self.hTimer = window.setTimeout(updateTimer, self._interval);
                    self.tick();
                }
                this.hTimer = window.setTimeout(updateTimer, self._interval);
            };
            this.stopTimer = function () {
                if (this.hTimer != null) window.clearTimeout(this.hTimer)
                this.hTimer = null;
            };
            this.tick = function () {
                var self = this;
                self._onTimeCallback();
            };
        }

        var timer = new Scheduler(1000, function () {
            var now = new Date();
                $("#echo").text(now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds()); 
            });
            timer.startTimer();

            var countdown = 60;
            var timer1 = new Scheduler(1000, function () 
            {
                countdown--;
                $("#echo1").text(countdown);
                if (countdown == 0) countdown = 60;
            });
            timer1.startTimer();
    </script>
</body>
</html>
