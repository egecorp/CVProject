﻿// Класс определяет модель одной Модели машины
var Car = function ()
{
  this.Id = -1;
  this.Brand = '';
  this.BrandId = 0;
  this.Name = undefined;
  this.CreateStamp = undefined;
  this.BodyTypeId = 0;
  this.BodyType = '';
  this.SeatsCount = undefined;
  this.Url = '';
  this.CarImageId = undefined;

  // Получим данные с сервера
  this.LoadFromServer = function (data)
  {
    this.Id = data.Id;
    this.BrandId = data.BrandId;
    this.BrandName = data.BrandName;
    this.Name = data.Name;
    this.CreateStamp = data.CreateStamp;
    this.BodyTypeId = data.BodyTypeId;
    this.BodyTypeName = data.BodyTypeName;
    this.SeatsCount = data.SeatsCount;
    this.Url = data.Url;
    this.CarImageId = data.CarImageId;
  }

  // Загрузим данные в форму
  this.ViewToCard = function ()
  {
    if (this.Id && (this.Id > 0))
    {
      $("#Car_Card_Id").val(this.Id);
    }
    else
    {
      $("#Car_Card_Id").val('Новая модель');
    }
    $("#Car_Card_BrandId").val(this.BrandId);
    $("#Car_Card_Name").val(this.Name);

    if (this.CreateStamp)
    {
      $("#Car_Card_CreateStamp").val(this.CreateStamp.substr(0, 16));
    }
    else
    {
      $("#Car_Card_CreateStamp").val(null);
    }

    $("#Car_Card_BodyTypeId").val(this.BodyTypeId);
    $("#Car_Card_SeatsCount").val(this.SeatsCount);
    $("#Car_Card_Url").val(this.Url);

    $('#Car_Card_CarImage').attr('src', me.server.GetImageUrl(this.CarImageId))


  }

  // Вернуть HTML строки в таблице моделей
  this.GetHTML = function ()
  {
    var tr = '<tr>';
    tr += '<td>' + this.Name + '</td>';
    tr += '<td>' + this.BrandName + '</td>';
    tr += '<td>' + this.BodyTypeName + '</td>';
    tr += '<td>' + this.SeatsCount + '</td>';

    tr += '<td><button onclick="Car_Edit_Function(' + this.Id + ')">ред.</button></td>';
    tr += '<td><button onclick="Car_Delete_Function(' + this.Id + ')">уд.</button></td>';

    tr += '</tr>';
    return tr;
  }

  //Считаем с формы и вернём объект с новыми данными
  this.SetFromCard = function ()
  {
    //this.Id = $("#Car_Card_Id").val();
    this.BrandId = $("#Car_Card_BrandId").val();
    this.Name = $("#Car_Card_Name").val();
    this.CreateStamp = $("#Car_Card_CreateStamp").val();
    this.BodyTypeId = $("#Car_Card_BodyTypeId").val();
    this.SeatsCount = $("#Car_Card_SeatsCount").val();
    this.Url = $("#Car_Card_Url").val();

    return {
      Id: +this.Id,
      BrandId: +this.BrandId,
      Name: this.Name,
      BodyTypeId: +this.BodyTypeId,
      SeatsCount: +this.SeatsCount,
      Url: this.Url,
      CarImageId: +this.CarImageId
    };
  }


}


