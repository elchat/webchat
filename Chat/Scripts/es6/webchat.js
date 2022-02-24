function getDate(date) {
    return (new Date(parseInt(date.substr(6))).toLocaleString());
}

function clearFilter() {
    $("#filter")[0].reset();
}

const historyPath = "/Chat/";
const hRequest = {
    getData: historyPath + "getData",
    getUsers: historyPath + "getUsers"
};

const ctx = {
    ajaxUrl: hRequest.getData,
    updateTable: function() {
        $.ajax({
            type: "POST",
            url: hRequest.getData,
            data: $("#filter").serialize()
        }).done(updateTableByData);
    }
};

function initDataTable(datatableOpts) {
    ctx.datatableApi = $("#datatable").DataTable(
        $.extend(true,
            datatableOpts,
            {
                ajax: {
                    type: "POST",
                    url: ctx.ajaxUrl,
                    dataSrc: ""
                },
                language: {
                    search: "Поиск",
                    processing: "В процессе...",
                    lengthMenu: "Показывать _MENU_ сообщений",
                    info: "Отображено от _START_ до _END_ из _TOTAL_",
                    infoEmpty: "Нет данных для отображения",
                    infoFiltered: "(всего отфильтровано из _MAX_ элементов)",
                    infoPostFix: "",
                    loadingRecords: "Загрузка...",
                    zeroRecords: "Нет элементов для отображения",
                    emptyTable: "Нет данных для отображения",
                    paginate: {
                        first: "Первый",
                        previous: "Пред.",
                        next: "След.",
                        last: "Последний"
                    },
                    aria: {
                        sortAscending: ": активировать, чтобы отсортировать столбец в порядке возрастания",
                        sortDescending: ": включить сортировку столбца в порядке убывания"
                    }
                },
                paging: true,
                info: true
            }
        ));
}


function updateTableByData(data) {
    ctx.datatableApi.clear().rows.add(data).draw();
}

$.ajaxSetup({
    converters: {
        "text json": function(stringData) {
            var json = JSON.parse(stringData);
            if (typeof json === 'object') {
                $(json).each(function() {
                    if (this.hasOwnProperty('DateTime')) {
                        this.DateTime = getDate(this.DateTime);
                    }
                });
            }
            return json;
        }
    }
});

$(document).ready(function() {

    const ChatModel = function() {
        const self = this;
        const minUserListSize = 10;

        self.isJoined = ko.observable(false);
        self.messages = ko.observableArray([]);
        self.messageText = ko.observable("");
        self.users = ko.observableArray([]);
        self.userListSize = ko.observable(minUserListSize);
    };

    const getUsers = function() {
        $.getJSON(hRequest.getUsers,
            {},
            function(events) {
                $("#user").html("");
                $("#user").get(0).options.add(new Option());
                $(events).each(function() {
                    $("#user").get(0).options.add(new Option(this));
                });
            });
    }

    $('#startDate').datetimepicker({
        format: 'Y-m-d H:i'
    });

    $('#endDate').datetimepicker({
        format: 'Y-m-d H:i'
    });

    initDataTable({
        "columns": [
            {
                "data": "Id"
            },
            {
                "data": "DateTime"
            },
            {
                "data": "UserName"
            },
            {
                "data": "Type"
            },
            {
                "data": "Message"
            }
        ],
        order: [
            [
                0,
                "asc"
            ]
        ],
        createdRow: function(row, data) {
            $(row).addClass(data.Type.toLowerCase());
        }
    });

    $('.dataTables_filter input').addClass('form-control formcontrol-sm');

    $('a[data-toggle="tab"]').on('shown.bs.tab',
        function(e) {
            if ($(e.target).attr("href") === "#hist")
                getUsers();
        });

    $.fn.dataTable.ext.errMode = 'throw';

    const chatModel = new ChatModel();
    ko.applyBindings(chatModel);
});