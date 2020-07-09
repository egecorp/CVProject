var me = {};

$(function () {
  me.server = new Server();
  InitEvents();

  GetPage(0);

});



function InitEvents()
{
  $("Main_Add_Button").click(function () {
    console.log("Нажата кнопка Добавить");
  });
};


var CurrentPage = 0;
var PageCount = 0;

function GetPage(PageNumber)
{
  function ViewTable(data) {

    var carList = [];

    CurrentPage = data.CurrentPage;
    PageCount = data.PageCount;

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
    '<li class="page-item ' + ((CurrentPage <= 0) ? 'disabled' : '') + ' ">' +
    ' <a class="page-link" ' + ((CurrentPage <= 0) ? '' : 'onclick="Nav_Prev()"') + ' href="#" tabindex="-1" aria-disabled="' + ((CurrentPage <= 0) ? 'true' : 'false') + '">Назад</a>' +
    '</li>' +
    '     <li class="page-item ' + ((CurrentPage == 0) ? 'active' : '') + ' "><a class="page-link" href="#" onclick="Nav_Open(0)">1</a></li>';

    for (var i = 1; i < PageCount; i++)
    {
      html += '     <li class="page-item ' + ((CurrentPage == i) ? 'active' : '') + '"><a class="page-link" href="#" onclick="Nav_Open(' + i +')">' + (i + 1) + '</a></li>';
    }

  html +=   '<li class="page-item ' + ((CurrentPage >= PageCount - 1) ? 'disabled' : '') + '">' +
    ' <a class="page-link" ' + ((CurrentPage >= PageCount -1) ? '' : 'onclick="Nav_Next()"') + '  href="#" aria-disabled="' + ((CurrentPage >= PageCount - 1) ? 'true' : 'false') + '">Далее</a>' +
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

    var oneCar = new Car();
    oneCar.LoadFromServer(data.OneCar);
    oneCar.ViewToCard();
    $("#Car_Card_Modal").modal('show');
  }
  me.server.GetOneCar(Id, ViewCard);  

  
}


function Car_Delete_Function(Id)
{
  if (confirm('Вы действительно хотите удалить модель?'))
  {
    function ReViewTable(data) {

      PageCount = data.PageCount;
      if ((CurrentPage >= PageCount) && (CurrentPage > 0)) CurrentPage--;
      Nav_Open(CurrentPage);
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
  if (CurrentPage >= PageCount) return;
  GetPage(CurrentPage + 1);
}


function Nav_Prev()
{
  if (CurrentPage <= 0) return;
  GetPage(CurrentPage - 1);
}