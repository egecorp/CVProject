// Класс описывает методы для связи с сервером
var Server = function ()
{
	this.APIURL = '/API';
	this.IMAGEURL = '/Image';

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
			if (!answer) {
				myCallBackFailFunction('Неизвестная ошибка выполнения запроса', myCallBackParam);
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

	// Функция загружает файл на сервер в файловую систему
	this.UploadImage = function (files, callBackFunction, callBackFailFunction, callBackParam) {
		var url = this.IMAGEURL + '/Upload';

		var myCallBackFunction = callBackFunction;
		var myCallBackFailFunction = callBackFailFunction || this.DefaultAlertFunction;
		var myCallBackParam = callBackParam;

		var fd = new FormData();
		fd.append("upload", files[0]);




		var request = $.ajax({
			url: url,
			data: fd,
			type: "POST",
			contentType: false,
			processData: false,
			context: window
		});

		request.done(function (jdata) {

			if (!jdata) {
				myCallBackFailFunction('Неизвестная ошибка выполнения запроса', myCallBackParam);
				return;
			}

			var answer = JSON.parse(jdata);

			if (answer.Error) {
				myCallBackFailFunction(answer.Error, myCallBackParam);
				return;
			}
			myCallBackFunction(answer, myCallBackParam);
		});

		request.fail(function (jqxhr, textStatus, error) {
			myCallBackFailFunction(error, myCallBackParam);
		});

	}

  // Получить страницу моделей с сервера
  this.GetCarPage = function (Page, GoodsResultCallBack)
	{
		var myGoodsResultCallBack = GoodsResultCallBack;
		function GoodResult(data)
		{
			myGoodsResultCallBack(data);
    }

		this.PerformPostRequest('Car/GetPage?Page=' + Page, {}, GoodResult);

  }

	// Получить одну модель с сервера
	this.GetOneCar = function (Id, GoodsResultCallBack) {
		var myGoodsResultCallBack = GoodsResultCallBack;
		function GoodResult(data) {
			myGoodsResultCallBack(data);
		}

		this.PerformPostRequest('Car/GetCar?Id=' + Id, {}, GoodResult);

	}

	// Удалить одну модель
	this.DeleteOneCar = function (Id, GoodsResultCallBack) {
		var myGoodsResultCallBack = GoodsResultCallBack;
		function GoodResult(data) {
			myGoodsResultCallBack(data);
		}
		this.PerformPostRequest('Car/DeleteCar?Id=' + Id, {}, GoodResult);
	}

	// Создать одну модель
	this.CreateOneCar = function (postData, GoodsResultCallBack) {
		var myGoodsResultCallBack = GoodsResultCallBack;
		function GoodResult(data) {
			myGoodsResultCallBack(data);
		}
		this.PerformPostRequest('Car/CreateCar', postData, GoodResult);
	}

	// Отредактировать одну модель
	this.EditOneCar = function (postData, GoodsResultCallBack) {
		var myGoodsResultCallBack = GoodsResultCallBack;
		function GoodResult(data) {
			myGoodsResultCallBack(data);
		}
		this.PerformPostRequest('Car/EditCar', postData, GoodResult);
	}

	// Вывод сообщения об ошибке по умолчанию
	this.DefaultAlertFunction = function (txt)
	{
		alert(txt);
	}

	// Вернить URL для картинки модели
	this.GetImageUrl = function (Id)
	{
		return this.IMAGEURL + '/Get?Id=' + Id;
  }

}



