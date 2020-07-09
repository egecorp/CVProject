// Класс описывает методы для связи с сервером
var Server = function ()
{
  this.APIURL = '/API';

	// Функция выполняет запрос и вызывает callBackFunction в случае успеха с параметром callBackParam и callBackFailFunction в случае ошибки
	this.PerformPostRequest = function (url, obj, callBackFunction, callBackFailFunction, callBackParam) {

		var myCallBackFunction = callBackFunction;
		var myCallBackFailFunction = callBackFailFunction || this.DefaultAlertFunction;
		var myCallBackParam = callBackParam;

		var url = this.APIURL + '/' + url;

		var request = $.ajax({
			url: url,
			data: JSON.stringify(obj),
			type: "POST",
			dataType: 'json',
			contentType: 'application/json; charset=utf-8',
			context: window
		});

		request.done(function (answer)
		{
			if (!answer)
			{
				myCallBackFailFunction('Произошла неизвестная ошибка', myCallBackParam);
				return;
			}

			if (answer.Error)
			{
				myCallBackFailFunction(answer.Error, myCallBackParam);
				return;
			}
			myCallBackFunction(answer, myCallBackParam);
		});

		request.fail(function (jqxhr, textStatus, error)
		{
			myCallBackFailFunction(error, myCallBackParam);
		});
	}


  this.GetCarPage = function (Page, GoodsResultCallBack)
	{
		var myGoodsResultCallBack = GoodsResultCallBack;
		function GoodResult(data)
		{
			myGoodsResultCallBack(data);
    }

		this.PerformPostRequest('Car/GetPage?Page=' + Page, {}, GoodResult);

  }

	this.GetOneCar = function (Id, GoodsResultCallBack) {
		var myGoodsResultCallBack = GoodsResultCallBack;
		function GoodResult(data) {
			myGoodsResultCallBack(data);
		}

		this.PerformPostRequest('Car/GetCar?Id=' + Id, {}, GoodResult);

	}

	this.DeleteOneCar = function (Id, GoodsResultCallBack) {
		var myGoodsResultCallBack = GoodsResultCallBack;
		function GoodResult(data) {
			myGoodsResultCallBack(data);
		}

		this.PerformPostRequest('Car/DeleteCar?Id=' + Id, {}, GoodResult);

	}
	

	this.DefaultAlertFunction = function (txt)
	{
		alert(txt);
  }

}



