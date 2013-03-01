$(function () {

	// carousel demo
	$('#myCarousel').carousel();

	// make code pretty
	$('pre').addClass("prettyprint");
	prettyPrint();

	// Add jquery validation
	if ($("form.validate").length > 0) {
		var form = $("form.validate");
		var validator = $.data(form[0], 'validator');
		var settngs = validator.settings;
		settngs.highlight = function (element) {
			$(element).parents("div.control-group").addClass("error");
		};
		settngs.unhighlight = function (element) {
			$(element).parents("div.control-group").removeClass("error");
		};
		form.find(".input-validation-error").closest(".control-group").addClass("error");
	}
})
