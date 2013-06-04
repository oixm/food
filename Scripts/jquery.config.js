// 设置 ajax 默认参数
$.ajaxSetup({
    async: true,
    cache: false
});

// 设置 datepicker 默认参数
$.datepicker.setDefaults({
    changeMonth: true,
    changeYear: true,
    showMonthAfterYear: true,
    dateFormat: "yy-mm-dd",
    dayNames: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
    dayNamesShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
    weekHeader: "周",
    firstDay: 1,
    monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
    monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
    prevText: "上一月",
    nextText: "下一月",
    currentText: "今天",
    closeText: "关闭",
    yearRange: "-40:+10"
});
