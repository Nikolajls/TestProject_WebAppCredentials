// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
  $("#add_calculate").click(function () { 
    
    let dataRequest = {
      Number1: $("#add_number1").val(),
      Number2: $("#add_number2").val()
    };

      $.ajax({
        url: "api/Calculation/Add",
        type: "POST",
        dataType: "json", 
        contentType: "application/json",
        data: JSON.stringify(dataRequest),
        success: (s) => $("#add_result").val(s.result),
        error: (error) => $("#add_result").val("Something went wrong!"),
      });
  });

  $("#minus_calculate").click(function () {

    let dataRequest = {
      Number: $("#minus_number").val(),
      MinusWith: $("#minus_with").val()
    };

    $.ajax({
      url: "api/Calculation/Minus",
      type: "POST",
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(dataRequest),
      success: (s) => $("#minus_result").val(s.result),
      error: (error) => $("#minus_result").val("Something went wrong!"),
    });
  });


  $("#multiply_calculate").click(function () {

    let dataRequest = {
      Number1: $("#multiply_number1").val(),
      Number2: $("#multiply_number2").val()
    };

    $.ajax({
      url: "api/Calculation/Multiply",
      type: "POST",
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(dataRequest),
      success: (s) => $("#multiply_result").val(s.result),
      error: (error) => $("#multiply_result").val("Something went wrong!"),
    });
  });

  $("#divide_calculate").click(function () {

    let dataRequest = {
      Number: $("#divide_number1").val(),
      DivideBy: $("#divide_number2").val()
    };

    $.ajax({
      url: "api/Calculation/Divide",
      type: "POST",
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(dataRequest),
      success: (s) => $("#divide_result").val(s.result),
      error: (error) => $("#divide_result").val("Something went wrong!"),
    });
  });

});

