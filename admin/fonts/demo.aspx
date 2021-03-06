<%@ Page Language="C#" AutoEventWireup="true" CodeFile="demo.aspx.cs" Inherits="admin_Ad_Table" %>

<!DOCTYPE html>
<html>
	<head>
        <title>ѡ������</title>
        <!--[if lt IE 9]><script language="javascript" type="text/javascript" src="//html5shim.googlecode.com/svn/trunk/html5.js"></script><![endif]-->
    <meta charset="UTF-8"><style>/*
 * Bootstrap v2.2.1
 *
 * Copyright 2012 Twitter, Inc
 * Licensed under the Apache License v2.0
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Designed and built with all the love in the world @twitter by @mdo and @fat.
 */
.clearfix {
  *zoom: 1;
}
.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
  line-height: 0;
}
.clearfix:after {
  clear: both;
}
html {
  font-size: 100%;
  -webkit-text-size-adjust: 100%;
  -ms-text-size-adjust: 100%;
}
a:focus {
  outline: thin dotted #333;
  outline: 5px auto -webkit-focus-ring-color;
  outline-offset: -2px;
}
a:hover,
a:active {
  outline: 0;
}
button,
input,
select,
textarea {
  margin: 0;
  font-size: 100%;
  vertical-align: middle;
}
button,
input {
  *overflow: visible;
  line-height: normal;
}
button::-moz-focus-inner,
input::-moz-focus-inner {
  padding: 0;
  border: 0;
}
body {
  margin: 0;
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-size: 14px;
  line-height: 20px;
  color: #333;
  background-color: #fff;
}
a {
  color: #08c;
  text-decoration: none;
}
a:hover {
  color: #005580;
  text-decoration: underline;
}
.row {
  margin-left: -20px;
  *zoom: 1;
}
.row:before,
.row:after {
  display: table;
  content: "";
  line-height: 0;
}
.row:after {
  clear: both;
}
[class*="span"] {
  float: left;
  min-height: 1px;
  margin-left: 20px;
}
.container,
.navbar-static-top .container,
.navbar-fixed-top .container,
.navbar-fixed-bottom .container {
  width: 940px;
}
.span12 {
  width: 940px;
}
.span11 {
  width: 860px;
}
.span10 {
  width: 780px;
}
.span9 {
  width: 700px;
}
.span8 {
  width: 620px;
}
.span7 {
  width: 540px;
}
.span6 {
  width: 460px;
}
.span5 {
  width: 380px;
}
.span4 {
  width: 300px;
}
.span3 {
  width: 220px;
}
.span2 {
  width: 140px;
}
.span1 {
  width: 60px;
}
[class*="span"].pull-right,
.row-fluid [class*="span"].pull-right {
  float: right;
}
.container {
  margin-right: auto;
  margin-left: auto;
  *zoom: 1;
}
.container:before,
.container:after {
  display: table;
  content: "";
  line-height: 0;
}
.container:after {
  clear: both;
}
p {
  margin: 0 0 10px;
}
.lead {
  margin-bottom: 20px;
  font-size: 21px;
  font-weight: 200;
  line-height: 30px;
}
small {
  font-size: 85%;
}
h1 {
  margin: 10px 0;
  font-family: inherit;
  font-weight: bold;
  line-height: 20px;
  color: inherit;
  text-rendering: optimizelegibility;
}
h1 small {
  font-weight: normal;
  line-height: 1;
  color: #999;
}
h1 {
  line-height: 40px;
}
h1 {
  font-size: 38.5px;
}
h1 small {
  font-size: 24.5px;
}
body {
  margin-top: 90px;
}
.header {
  position: fixed;
  top: 0;
  left: 50%;
  margin-left: -480px;
  background-color: #fff;
  border-bottom: 1px solid #ddd;
  padding-top: 10px;
  z-index: 10;
}
.footer {
  color: #ddd;
  font-size: 12px;
  text-align: center;
  margin-top: 20px;
}
.footer a {
  color: #ccc;
  text-decoration: underline;
}
.the-icons {
  font-size: 14px;
  line-height: 24px;
}
.switch {
  position: absolute;
  right: 0;
  bottom: 10px;
  color: #666;
}
.switch input {
  margin-right: 0.3em;
}
.codesOn .i-name {
  display: none;
}
.codesOn .i-code {
  display: inline;
}
.i-code {
  display: none;
}
@font-face {
      font-family: 'fontello';
      src: url('./font/fontello.eot?48418611');
      src: url('./font/fontello.eot?48418611#iefix') format('embedded-opentype'),
           url('./font/fontello.woff?48418611') format('woff'),
           url('./font/fontello.ttf?48418611') format('truetype'),
           url('./font/fontello.svg?48418611#fontello') format('svg');
      font-weight: normal;
      font-style: normal;
    }
     
     
    .demo-icon
    {
      font-family: "fontello";
      font-style: normal;
      font-weight: normal;
      speak: none;
     
      display: inline-block;
      text-decoration: inherit;
      width: 1em;
      margin-right: .2em;
      text-align: center;
      /* opacity: .8; */
     
      /* For safety - reset parent styles, that can break glyph codes*/
      font-variant: normal;
      text-transform: none;
     
      /* fix buttons height, for twitter bootstrap */
      line-height: 1em;
     
      /* Animation center compensation - margins should be symmetric */
      /* remove if not needed */
      margin-left: .2em;
     
      /* You can be more comfortable with increased icons size */
      /* font-size: 120%; */
     
      /* Font smoothing. That was taken from TWBS */
      -webkit-font-smoothing: antialiased;
      -moz-osx-font-smoothing: grayscale;
     
      /* Uncomment for 3D effect */
      /* text-shadow: 1px 1px 1px rgba(127, 127, 127, 0.3); */
    }
     </style>
    <link rel="stylesheet" href="css/animation.css"><!--[if IE 7]><link rel="stylesheet" href="css/fontello-ie7.css"><![endif]-->
    <script>
      function toggleCodes(on) {
        var obj = document.getElementById('icons');
        
        if (on) {
          obj.className += ' codesOn';
        } else {
          obj.className = obj.className.replace(' codesOn', '');
        }
      }
      
    </script>
         <script src="../js/jquery-1.11.2.min.js" type="text/javascript" charset="utf-8"></script>
	
  </head>
	<body class="body-color">
        <form id="form1" runat="server">
		 <div class="container header">
      <h1>
        ѡ������
         <small>font demo</small>
      </h1>
      <label class="switch">
        <input type="checkbox" onclick="toggleCodes(this.checked)">show codes
      </label>
    </div>
    <div id="icons" class="container">
      <div class="row">
        <div title="Code: 0x2605" class="the-icons span3"><i class="demo-icon ficon-star">&#x2605;</i> <span class="i-name">ficon-star</span><span class="i-code">0x2605</span></div>
        <div title="Code: 0x2606" class="the-icons span3"><i class="demo-icon ficon-star-empty">&#x2606;</i> <span class="i-name">ficon-star-empty</span><span class="i-code">0x2606</span></div>
        <div title="Code: 0x2661" class="the-icons span3"><i class="demo-icon ficon-heart-empty">&#x2661;</i> <span class="i-name">ficon-heart-empty</span><span class="i-code">0x2661</span></div>
        <div title="Code: 0x2665" class="the-icons span3"><i class="demo-icon ficon-heart">&#x2665;</i> <span class="i-name">ficon-heart</span><span class="i-code">0x2665</span></div>
      </div>
      <div class="row">
        <div title="Code: 0x2713" class="the-icons span3"><i class="demo-icon ficon-ok">&#x2713;</i> <span class="i-name">ficon-ok</span><span class="i-code">0x2713</span></div>
        <div title="Code: 0x2715" class="the-icons span3"><i class="demo-icon ficon-cancel">&#x2715;</i> <span class="i-name">ficon-cancel</span><span class="i-code">0x2715</span></div>
        <div title="Code: 0xe75c" class="the-icons span3"><i class="demo-icon ficon-down-open">&#xe75c;</i> <span class="i-name">ficon-down-open</span><span class="i-code">0xe75c</span></div>
        <div title="Code: 0xe75d" class="the-icons span3"><i class="demo-icon ficon-left-open">&#xe75d;</i> <span class="i-name">ficon-left-open</span><span class="i-code">0xe75d</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe75e" class="the-icons span3"><i class="demo-icon ficon-right-open">&#xe75e;</i> <span class="i-name">ficon-right-open</span><span class="i-code">0xe75e</span></div>
        <div title="Code: 0xe75f" class="the-icons span3"><i class="demo-icon ficon-up-open">&#xe75f;</i> <span class="i-name">ficon-up-open</span><span class="i-code">0xe75f</span></div>
        <div title="Code: 0xe800" class="the-icons span3"><i class="demo-icon ficon-chanpinjieshao">&#xe800;</i> <span class="i-name">ficon-chanpinjieshao</span><span class="i-code">0xe800</span></div>
        <div title="Code: 0xe801" class="the-icons span3"><i class="demo-icon ficon-checked-fill">&#xe801;</i> <span class="i-name">ficon-checked-fill</span><span class="i-code">0xe801</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe802" class="the-icons span3"><i class="demo-icon ficon-chilunhover">&#xe802;</i> <span class="i-name">ficon-chilunhover</span><span class="i-code">0xe802</span></div>
        <div title="Code: 0xe803" class="the-icons span3"><i class="demo-icon ficon-deng">&#xe803;</i> <span class="i-name">ficon-deng</span><span class="i-code">0xe803</span></div>
        <div title="Code: 0xe804" class="the-icons span3"><i class="demo-icon ficon-cuxiaohuodong">&#xe804;</i> <span class="i-name">ficon-cuxiaohuodong</span><span class="i-code">0xe804</span></div>
        <div title="Code: 0xe805" class="the-icons span3"><i class="demo-icon ficon-dianhua">&#xe805;</i> <span class="i-name">ficon-dianhua</span><span class="i-code">0xe805</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe806" class="the-icons span3"><i class="demo-icon ficon-dingdan">&#xe806;</i> <span class="i-name">ficon-dingdan</span><span class="i-code">0xe806</span></div>
        <div title="Code: 0xe807" class="the-icons span3"><i class="demo-icon ficon-diqiu">&#xe807;</i> <span class="i-name">ficon-diqiu</span><span class="i-code">0xe807</span></div>
        <div title="Code: 0xe808" class="the-icons span3"><i class="demo-icon ficon-duanxin">&#xe808;</i> <span class="i-name">ficon-duanxin</span><span class="i-code">0xe808</span></div>
        <div title="Code: 0xe809" class="the-icons span3"><i class="demo-icon ficon-fenxiao">&#xe809;</i> <span class="i-name">ficon-fenxiao</span><span class="i-code">0xe809</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe80a" class="the-icons span3"><i class="demo-icon ficon-financial-manage">&#xe80a;</i> <span class="i-name">ficon-financial-manage</span><span class="i-code">0xe80a</span></div>
        <div title="Code: 0xe80b" class="the-icons span3"><i class="demo-icon ficon-fuzhi">&#xe80b;</i> <span class="i-name">ficon-fuzhi</span><span class="i-code">0xe80b</span></div>
        <div title="Code: 0xe80c" class="the-icons span3"><i class="demo-icon ficon-fuzhi-copy">&#xe80c;</i> <span class="i-name">ficon-fuzhi-copy</span><span class="i-code">0xe80c</span></div>
        <div title="Code: 0xe80d" class="the-icons span3"><i class="demo-icon ficon-gengxin">&#xe80d;</i> <span class="i-name">ficon-gengxin</span><span class="i-code">0xe80d</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe80e" class="the-icons span3"><i class="demo-icon ficon-gouwuche">&#xe80e;</i> <span class="i-name">ficon-gouwuche</span><span class="i-code">0xe80e</span></div>
        <div title="Code: 0xe80f" class="the-icons span3"><i class="demo-icon ficon-huifu">&#xe80f;</i> <span class="i-name">ficon-huifu</span><span class="i-code">0xe80f</span></div>
        <div title="Code: 0xe810" class="the-icons span3"><i class="demo-icon ficon-huiyuan">&#xe810;</i> <span class="i-name">ficon-huiyuan</span><span class="i-code">0xe810</span></div>
        <div title="Code: 0xe811" class="the-icons span3"><i class="demo-icon ficon-jianli">&#xe811;</i> <span class="i-name">ficon-jianli</span><span class="i-code">0xe811</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe812" class="the-icons span3"><i class="demo-icon ficon-kecheng">&#xe812;</i> <span class="i-name">ficon-kecheng</span><span class="i-code">0xe812</span></div>
        <div title="Code: 0xe813" class="the-icons span3"><i class="demo-icon ficon-kuozhanicon">&#xe813;</i> <span class="i-name">ficon-kuozhanicon</span><span class="i-code">0xe813</span></div>
        <div title="Code: 0xe814" class="the-icons span3"><i class="demo-icon ficon-lanmu">&#xe814;</i> <span class="i-name">ficon-lanmu</span><span class="i-code">0xe814</span></div>
        <div title="Code: 0xe815" class="the-icons span3"><i class="demo-icon ficon-lanmu1">&#xe815;</i> <span class="i-name">ficon-lanmu1</span><span class="i-code">0xe815</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe816" class="the-icons span3"><i class="demo-icon ficon-lanmu2">&#xe816;</i> <span class="i-name">ficon-lanmu2</span><span class="i-code">0xe816</span></div>
        <div title="Code: 0xe817" class="the-icons span3"><i class="demo-icon ficon-lianxi">&#xe817;</i> <span class="i-name">ficon-lianxi</span><span class="i-code">0xe817</span></div>
        <div title="Code: 0xe818" class="the-icons span3"><i class="demo-icon ficon-list">&#xe818;</i> <span class="i-name">ficon-list</span><span class="i-code">0xe818</span></div>
        <div title="Code: 0xe819" class="the-icons span3"><i class="demo-icon ficon-liuchengbutongguo">&#xe819;</i> <span class="i-name">ficon-liuchengbutongguo</span><span class="i-code">0xe819</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe81a" class="the-icons span3"><i class="demo-icon ficon-mima">&#xe81a;</i> <span class="i-name">ficon-mima</span><span class="i-code">0xe81a</span></div>
        <div title="Code: 0xe81b" class="the-icons span3"><i class="demo-icon ficon-o11">&#xe81b;</i> <span class="i-name">ficon-o11</span><span class="i-code">0xe81b</span></div>
        <div title="Code: 0xe81c" class="the-icons span3"><i class="demo-icon ficon-photo">&#xe81c;</i> <span class="i-name">ficon-photo</span><span class="i-code">0xe81c</span></div>
        <div title="Code: 0xe81d" class="the-icons span3"><i class="demo-icon ficon-qiehuan">&#xe81d;</i> <span class="i-name">ficon-qiehuan</span><span class="i-code">0xe81d</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe81e" class="the-icons span3"><i class="demo-icon ficon-fanxuan">&#xe81e;</i> <span class="i-name">ficon-fanxuan</span><span class="i-code">0xe81e</span></div>
        <div title="Code: 0xe81f" class="the-icons span3"><i class="demo-icon ficon-qingkong">&#xe81f;</i> <span class="i-name">ficon-qingkong</span><span class="i-code">0xe81f</span></div>
        <div title="Code: 0xe820" class="the-icons span3"><i class="demo-icon ficon-queren">&#xe820;</i> <span class="i-name">ficon-queren</span><span class="i-code">0xe820</span></div>
        <div title="Code: 0xe821" class="the-icons span3"><i class="demo-icon ficon-renzhengtongguo">&#xe821;</i> <span class="i-name">ficon-renzhengtongguo</span><span class="i-code">0xe821</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe822" class="the-icons span3"><i class="demo-icon ficon-riqi">&#xe822;</i> <span class="i-name">ficon-riqi</span><span class="i-code">0xe822</span></div>
        <div title="Code: 0xe823" class="the-icons span3"><i class="demo-icon ficon-shanchu">&#xe823;</i> <span class="i-name">ficon-shanchu</span><span class="i-code">0xe823</span></div>
        <div title="Code: 0xe824" class="the-icons span3"><i class="demo-icon ficon-shangchuan">&#xe824;</i> <span class="i-name">ficon-shangchuan</span><span class="i-code">0xe824</span></div>
        <div title="Code: 0xe825" class="the-icons span3"><i class="demo-icon ficon-shangpuzhongxin">&#xe825;</i> <span class="i-name">ficon-shangpuzhongxin</span><span class="i-code">0xe825</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe826" class="the-icons span3"><i class="demo-icon ficon-shezhi">&#xe826;</i> <span class="i-name">ficon-shezhi</span><span class="i-code">0xe826</span></div>
        <div title="Code: 0xe827" class="the-icons span3"><i class="demo-icon ficon-shop">&#xe827;</i> <span class="i-name">ficon-shop</span><span class="i-code">0xe827</span></div>
        <div title="Code: 0xe828" class="the-icons span3"><i class="demo-icon ficon-shouji">&#xe828;</i> <span class="i-name">ficon-shouji</span><span class="i-code">0xe828</span></div>
        <div title="Code: 0xe829" class="the-icons span3"><i class="demo-icon ficon-shouye">&#xe829;</i> <span class="i-name">ficon-shouye</span><span class="i-code">0xe829</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe82a" class="the-icons span3"><i class="demo-icon ficon-shuaxin">&#xe82a;</i> <span class="i-name">ficon-shuaxin</span><span class="i-code">0xe82a</span></div>
        <div title="Code: 0xe82b" class="the-icons span3"><i class="demo-icon ficon-shujufenxi">&#xe82b;</i> <span class="i-name">ficon-shujufenxi</span><span class="i-code">0xe82b</span></div>
        <div title="Code: 0xe82c" class="the-icons span3"><i class="demo-icon ficon-shujutanshuju">&#xe82c;</i> <span class="i-name">ficon-shujutanshuju</span><span class="i-code">0xe82c</span></div>
        <div title="Code: 0xe82d" class="the-icons span3"><i class="demo-icon ficon-shujutongji">&#xe82d;</i> <span class="i-name">ficon-shujutongji</span><span class="i-code">0xe82d</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe82e" class="the-icons span3"><i class="demo-icon ficon-tianjia">&#xe82e;</i> <span class="i-name">ficon-tianjia</span><span class="i-code">0xe82e</span></div>
        <div title="Code: 0xe82f" class="the-icons span3"><i class="demo-icon ficon-tixian">&#xe82f;</i> <span class="i-name">ficon-tixian</span><span class="i-code">0xe82f</span></div>
        <div title="Code: 0xe830" class="the-icons span3"><i class="demo-icon ficon-tongzhi">&#xe830;</i> <span class="i-name">ficon-tongzhi</span><span class="i-code">0xe830</span></div>
        <div title="Code: 0xe831" class="the-icons span3"><i class="demo-icon ficon-tuichu">&#xe831;</i> <span class="i-name">ficon-tuichu</span><span class="i-code">0xe831</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe832" class="the-icons span3"><i class="demo-icon ficon-uploading">&#xe832;</i> <span class="i-name">ficon-uploading</span><span class="i-code">0xe832</span></div>
        <div title="Code: 0xe833" class="the-icons span3"><i class="demo-icon ficon-user">&#xe833;</i> <span class="i-name">ficon-user</span><span class="i-code">0xe833</span></div>
        <div title="Code: 0xe834" class="the-icons span3"><i class="demo-icon ficon-wait">&#xe834;</i> <span class="i-name">ficon-wait</span><span class="i-code">0xe834</span></div>
        <div title="Code: 0xe835" class="the-icons span3"><i class="demo-icon ficon-weixin">&#xe835;</i> <span class="i-name">ficon-weixin</span><span class="i-code">0xe835</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe836" class="the-icons span3"><i class="demo-icon ficon-weixin1">&#xe836;</i> <span class="i-name">ficon-weixin1</span><span class="i-code">0xe836</span></div>
        <div title="Code: 0xe837" class="the-icons span3"><i class="demo-icon ficon-weizhi">&#xe837;</i> <span class="i-name">ficon-weizhi</span><span class="i-code">0xe837</span></div>
        <div title="Code: 0xe838" class="the-icons span3"><i class="demo-icon ficon-wenzhang">&#xe838;</i> <span class="i-name">ficon-wenzhang</span><span class="i-code">0xe838</span></div>
        <div title="Code: 0xe839" class="the-icons span3"><i class="demo-icon ficon-withdraw">&#xe839;</i> <span class="i-name">ficon-withdraw</span><span class="i-code">0xe839</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe83a" class="the-icons span3"><i class="demo-icon ficon-xingbienv">&#xe83a;</i> <span class="i-name">ficon-xingbienv</span><span class="i-code">0xe83a</span></div>
        <div title="Code: 0xe83b" class="the-icons span3"><i class="demo-icon ficon-xingming">&#xe83b;</i> <span class="i-name">ficon-xingming</span><span class="i-code">0xe83b</span></div>
        <div title="Code: 0xe83c" class="the-icons span3"><i class="demo-icon ficon-xiugai">&#xe83c;</i> <span class="i-name">ficon-xiugai</span><span class="i-code">0xe83c</span></div>
        <div title="Code: 0xe83d" class="the-icons span3"><i class="demo-icon ficon-yidong">&#xe83d;</i> <span class="i-name">ficon-yidong</span><span class="i-code">0xe83d</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe83e" class="the-icons span3"><i class="demo-icon ficon-yonghuming">&#xe83e;</i> <span class="i-name">ficon-yonghuming</span><span class="i-code">0xe83e</span></div>
        <div title="Code: 0xe83f" class="the-icons span3"><i class="demo-icon ficon-yonghuzhongxin">&#xe83f;</i> <span class="i-name">ficon-yonghuzhongxin</span><span class="i-code">0xe83f</span></div>
        <div title="Code: 0xe840" class="the-icons span3"><i class="demo-icon ficon-youxi">&#xe840;</i> <span class="i-name">ficon-youxi</span><span class="i-code">0xe840</span></div>
        <div title="Code: 0xe841" class="the-icons span3"><i class="demo-icon ficon-youxiang">&#xe841;</i> <span class="i-name">ficon-youxiang</span><span class="i-code">0xe841</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe842" class="the-icons span3"><i class="demo-icon ficon-youxiang1">&#xe842;</i> <span class="i-name">ficon-youxiang1</span><span class="i-code">0xe842</span></div>
        <div title="Code: 0xe843" class="the-icons span3"><i class="demo-icon ficon-youxiang2">&#xe843;</i> <span class="i-name">ficon-youxiang2</span><span class="i-code">0xe843</span></div>
        <div title="Code: 0xe844" class="the-icons span3"><i class="demo-icon ficon-yuncipancds">&#xe844;</i> <span class="i-name">ficon-yuncipancds</span><span class="i-code">0xe844</span></div>
        <div title="Code: 0xe845" class="the-icons span3"><i class="demo-icon ficon-yunying">&#xe845;</i> <span class="i-name">ficon-yunying</span><span class="i-code">0xe845</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe846" class="the-icons span3"><i class="demo-icon ficon-yzyonghuguanli">&#xe846;</i> <span class="i-name">ficon-yzyonghuguanli</span><span class="i-code">0xe846</span></div>
        <div title="Code: 0xe847" class="the-icons span3"><i class="demo-icon ficon-zhandianlanmu">&#xe847;</i> <span class="i-name">ficon-zhandianlanmu</span><span class="i-code">0xe847</span></div>
        <div title="Code: 0xe848" class="the-icons span3"><i class="demo-icon ficon-zhuangtai">&#xe848;</i> <span class="i-name">ficon-zhuangtai</span><span class="i-code">0xe848</span></div>
        <div title="Code: 0xe849" class="the-icons span3"><i class="demo-icon ficon-zhuanhuan">&#xe849;</i> <span class="i-name">ficon-zhuanhuan</span><span class="i-code">0xe849</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe84a" class="the-icons span3"><i class="demo-icon ficon-zoomout">&#xe84a;</i> <span class="i-name">ficon-zoomout</span><span class="i-code">0xe84a</span></div>
        <div title="Code: 0xe84b" class="the-icons span3"><i class="demo-icon ficon-ananzui">&#xe84b;</i> <span class="i-name">ficon-ananzui</span><span class="i-code">0xe84b</span></div>
        <div title="Code: 0xe84c" class="the-icons span3"><i class="demo-icon ficon-anquan">&#xe84c;</i> <span class="i-name">ficon-anquan</span><span class="i-code">0xe84c</span></div>
        <div title="Code: 0xe84d" class="the-icons span3"><i class="demo-icon ficon-arrow-left">&#xe84d;</i> <span class="i-name">ficon-arrow-left</span><span class="i-code">0xe84d</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe84e" class="the-icons span3"><i class="demo-icon ficon-automatic-configuration">&#xe84e;</i> <span class="i-name">ficon-automatic-configuration</span><span class="i-code">0xe84e</span></div>
        <div title="Code: 0xe84f" class="the-icons span3"><i class="demo-icon ficon-back">&#xe84f;</i> <span class="i-name">ficon-back</span><span class="i-code">0xe84f</span></div>
        <div title="Code: 0xe850" class="the-icons span3"><i class="demo-icon ficon-backup">&#xe850;</i> <span class="i-name">ficon-backup</span><span class="i-code">0xe850</span></div>
        <div title="Code: 0xe851" class="the-icons span3"><i class="demo-icon ficon-chakan">&#xe851;</i> <span class="i-name">ficon-chakan</span><span class="i-code">0xe851</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xe852" class="the-icons span3"><i class="demo-icon ficon-changyongxiangmu">&#xe852;</i> <span class="i-name">ficon-changyongxiangmu</span><span class="i-code">0xe852</span></div>
        <div title="Code: 0xe854" class="the-icons span3"><i class="demo-icon ficon-iconfont-baidu">&#xe854;</i> <span class="i-name">ficon-iconfont-baidu</span><span class="i-code">0xe854</span></div>
        <div title="Code: 0xe855" class="the-icons span3"><i class="demo-icon ficon-iconfont-qq">&#xe855;</i> <span class="i-name">ficon-iconfont-qq</span><span class="i-code">0xe855</span></div>
        <div title="Code: 0xf0c9" class="the-icons span3"><i class="demo-icon ficon-menu">&#xf0c9;</i> <span class="i-name">ficon-menu</span><span class="i-code">0xf0c9</span></div>
      </div>
      <div class="row">
        <div title="Code: 0xf104" class="the-icons span3"><i class="demo-icon ficon-angle-left">&#xf104;</i> <span class="i-name">ficon-angle-left</span><span class="i-code">0xf104</span></div>
        <div title="Code: 0xf105" class="the-icons span3"><i class="demo-icon ficon-angle-right">&#xf105;</i> <span class="i-name">ficon-angle-right</span><span class="i-code">0xf105</span></div>
        <div title="Code: 0xf106" class="the-icons span3"><i class="demo-icon ficon-angle-up">&#xf106;</i> <span class="i-name">ficon-angle-up</span><span class="i-code">0xf106</span></div>
        <div title="Code: 0xf107" class="the-icons span3"><i class="demo-icon ficon-angle-down">&#xf107;</i> <span class="i-name">ficon-angle-down</span><span class="i-code">0xf107</span></div>
      </div>
      <div class="row">
        <div title="Code: 0x1f44d" class="the-icons span3"><i class="demo-icon ficon-thumbs-up">&#x1f44d;</i> <span class="i-name">ficon-thumbs-up</span><span class="i-code">0x1f44d</span></div>
        <div title="Code: 0x1f44e" class="the-icons span3"><i class="demo-icon ficon-thumbs-down">&#x1f44e;</i> <span class="i-name">ficon-thumbs-down</span><span class="i-code">0x1f44e</span></div>
      </div>
    </div>
  <script type="text/javascript">
      $(".the-icons").click(function () {
          var classname = $(this).find(".i-name").html();

          window.opener.document.all('<%=editname %>').value = classname;
          window.opener.document.all('<%=editname %>_pic').className = "ficon " + classname;
	window.close();
      });
  </script>
		</form>
	</body>
</html>
