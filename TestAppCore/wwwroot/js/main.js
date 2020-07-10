var me = {};

$(function () {
  me.server = new Server();
  me.currentCar = null;
  me.CurrentPage = 0;
  me.PageCount = 0;

  InitEvents();

  GetPage(0);

});



function InitEvents()
{
  $("Main_Add_Button").click(function () {
    console.log("Нажата кнопка Добавить");
  });

  $("#Cars_Table_Add_Button").click(function () {

    me.currentCar = new Car();
    me.currentCar.ViewToCard();
    $("#Car_Card_Modal").modal('show');
   
  });

  $("#Car_Card_Save_Button").click(function ()
  {
    if (!me.currentCar) return;
    var postData = me.currentCar.SetFromCard();

    console.log(postData);
    if (me.currentCar.Id < 0)
    {
      console.log('Create');
    }
    else
    {
      console.log('Save');
    }
    

  });
};




function GetPage(PageNumber)
{
  function ViewTable(data) {

    var carList = [];

    me.CurrentPage = data.CurrentPage;
    me.PageCount = data.PageCount;

    for (var i in data.CarList)
    {
      var cl = data.CarList[i];
      if (!cl) continue;
      var oneCar = new Car();
      oneCar.LoadFromServer(cl);
      carList.push(oneCar);
    }
    LoadDataToTable(carList);
    LoadDataToNav();

    console.log(data);
    
  }
  me.server.GetCarPage(PageNumber, ViewTable);  
}


function LoadDataToTable(carList)
{
  var html = "";
  for (var i in carList)
  {
    var oneCar = carList[i];
    if (!oneCar) continue;
    html += oneCar.GetHTML();
  }

  $("#Cars_Table_Body").html(html);
}


function LoadDataToNav()
{
  var html = '' +
    '<li class="page-item ' + ((me.CurrentPage <= 0) ? 'disabled' : '') + ' ">' +
    ' <a class="page-link" ' + ((me.CurrentPage <= 0) ? '' : 'onclick="Nav_Prev()"') + ' href="#" tabindex="-1" aria-disabled="' + ((me.CurrentPage <= 0) ? 'true' : 'false') + '">Назад</a>' +
    '</li>' +
    '     <li class="page-item ' + ((me.CurrentPage == 0) ? 'active' : '') + ' "><a class="page-link" href="#" onclick="Nav_Open(0)">1</a></li>';

  for (var i = 1; i < me.PageCount; i++)
    {
    html += '     <li class="page-item ' + ((me.CurrentPage == i) ? 'active' : '') + '"><a class="page-link" href="#" onclick="Nav_Open(' + i +')">' + (i + 1) + '</a></li>';
    }

  html += '<li class="page-item ' + ((me.CurrentPage >= me.PageCount - 1) ? 'disabled' : '') + '">' +
    ' <a class="page-link" ' + ((me.CurrentPage >= me.PageCount - 1) ? '' : 'onclick="Nav_Next()"') + '  href="#" aria-disabled="' + ((me.CurrentPage >= me.PageCount - 1) ? 'true' : 'false') + '">Далее</a>' +
            '</li>';
  $("#Cars_Table_Nav").html(html);
}

function Car_Edit_Function(Id)
{
  function ViewCard(data) {

    if (!data.OneCar)
    {
      alert("Ошибка получения данных");
      return;
    }

    me.currentCar = new Car();
    me.currentCar.LoadFromServer(data.OneCar);
    me.currentCar.ViewToCard();
    $("#Car_Card_Modal").modal('show');
  }
  me.server.GetOneCar(Id, ViewCard);  

  
}


function Car_Delete_Function(Id)
{
  if (confirm('Вы действительно хотите удалить модель?'))
  {
    function ReViewTable(data) {

      me.PageCount = data.PageCount;
      if ((me.CurrentPage >= me.PageCount) && (me.CurrentPage > 0)) me.CurrentPage--;
      Nav_Open(me.CurrentPage);
    }
    me.server.DeleteOneCar(Id, ReViewTable);  

  }

}


function Nav_Open(PageNumber)
{
  GetPage(PageNumber);
}


function Nav_Next()
{
  if (me.CurrentPage >= me.PageCount) return;
  GetPage(me.CurrentPage + 1);
}


function Nav_Prev()
{
  if (me.CurrentPage <= 0) return;
  GetPage(me.CurrentPage - 1);
}