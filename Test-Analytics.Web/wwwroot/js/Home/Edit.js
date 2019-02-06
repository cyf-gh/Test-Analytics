$(function () {
    $('#myTab li:first-child a').tab('show')
})

// $(window).bind('beforeunload', function () { });

// $(window).bind('onload', function () { });

function Dictionary() {
    this.data = new Array();

    this.put = function (key, value) {
        this.data[key] = value;
    };

    this.get = function (key) {
        return this.data[key];
    };

    this.remove = function (key) {
        this.data[key] = null;
    };

    this.removeAll = function () {
        delete this.data;
        this.data = new Array();
    }

    this.isEmpty = function () {
        return this.data.length == 0;
    };

    this.size = function () {
        return this.data.length;
    };
}

function saveCapas() {
    var capas = new Array();
    $("#id-capa-cards").children("div").each(function (i) {
        var capaName = $(this).children("h5").text();
        var capaDesc = $(this).children("div[class=card-body]").children("input[id=capa-desc]").val();
        var capafof = $(this).children("div[class=card-body]").find("input[name=name-fof" + capaName + "]:checked").val();
        var capaImpact = $(this).children("div[class=card-body]").find("input[name=name-impact" + capaName + "]:checked").val();
        var capaTags = $(this).children("div[class=card-body]").children("input[id=capa-tags]").val();

        var capaAttr = $(this).children("h5").attr("attr");
        var capaComp = $(this).children("h5").attr("comp");

        var capa = {
            ProjectId: $('div[id=id-meta-info]').attr("project-id"),
            Tags: capaTags,
            Name: capaName,
            Desc: capaDesc,
            FoF: capafof,
            Impact: capaImpact,
            Attr: capaAttr,
            Comp: capaComp
        };
        capas[i] = capa;
    });
    console.log(capas);
    $.ajax({
        url: '/project/save-project-capas',
        type: 'POST',
        data: JSON.stringify(capas),
        async: false,
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (returndata) { },
        error: function (returndata) { }
    });
}

function saveProject() {
    $.ajax({
        url: '/project/save-project',
        type: 'POST',
        data: JSON.stringify({
            Id: $('div[id=id-meta-info]').attr("project-id"),
            Name: $('#project-name').val(),
            Description: $('#project-desc').val(),
            Owners: $('#project-owners').val(),
            Editors: $('#project-editors').val(),
            Viewers: $('#project-viewers').val(),
            IsPublic: $('#project-is-public')[0].checked
        }),
        async: false,
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (returndata) { },
        error: function (returndata) { }
    });
}

function saveAttrs() {
    var attrs = new Array();
    $("#img-card-group-attr div[class=card]").each(function (i) {
        var attrName = $(this).children("h5").text();
        var attrDesc = $(this).children("div[class=card-body]").children("input[id=id-group-attr-desc]").val();
        var attrTags = $(this).children("div[class=card-body]").children("input[id=id-group-attr-tags]").val();
        var attrChecker = $(this).children("div").children("input[id=id-group-attr-checker]")[0].checked;
        var attr = {
            ProjectId: $('div[id=id-meta-info]').attr("project-id"),
            Name: attrName,
            Desc: attrDesc,
            Tags: attrTags,
            IsSuffi: attrChecker
        };
        attrs[i] = attr;
    });
    console.log(attrs);
    $.ajax({
        url: '/project/save-project-attrs',
        type: 'POST',
        data: JSON.stringify(attrs),
        async: false,
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (returndata) { },
        error: function (returndata) { }
    });
}

function saveComps() {
    var comps = new Array();
    $("#img-card-group-comp div[class=card]").each(function (i) {
        var compName = $(this).children("h5").text();
        var compDesc = $(this).children("div[class=card-body]").children("input[id=id-group-comp-desc]").val();
        var compTags = $(this).children("div[class=card-body]").children("input[id=id-group-comp-tags]").val();
        var compChecker = $(this).children("div").children("input[id=id-group-comp-checker]")[0].checked;
        var comp = {
            ProjectId: $('div[id=id-meta-info]').attr("project-id"),
            Name: compName,
            Desc: compDesc,
            Tags: compTags,
            IsSuffi: compChecker
        };
        comps[i] = comp;
    });
    console.log(comps);
    $.ajax({
        url: '/project/save-project-comps',
        type: 'POST',
        data: JSON.stringify(comps),
        async: false,
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (returndata) { },
        error: function (returndata) { }
    });
}

function pushSave() {
    $.ajax({
        url: '/project/push-save',
        type: 'POST',
        async: false,
        cache: false,
        success: function (returndata) { },
        error: function (returndata) { }
    });
}

function saveAll() {
    saveComps();
    saveAttrs();
    saveCapas();
    saveProject();
    pushSave();
}

function deleteProject() {
    if (confirm("Are you sure to delete current project?")) {
        if (confirm("Confirm again.")) {
            $.ajax({
                url: '/project/delete-project',
                type: 'POST',
                data: JSON.stringify({
                    Id: $('div[id=id-meta-info]').attr("project-id"),
                    Name: $('#project-name').val(),
                    Description: $('#project-desc').val(),
                    Owners: $('#project-owners').val(),
                    Editors: $('#project-editors').val(),
                    Viewers: $('#project-viewers').val(),
                    IsPublic: $('#project-is-public')[0].checked
                }),
                async: false,
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (returndata) {
                    alert("Project removed.");
                    window.location.href = "/project/index";
                },
                error: function (returndata) { }
            });
        }
    }
}

var lastA = null;
function showCurrentCapability(obj) {
    if (lastA != null) {
        $(lastA).attr("class", "")
    }
    $(obj).tab('show');
    lastA = obj;
    curAttr = $(obj).attr(keyAttr);
    curComp = $(obj).attr(keyComp);
}

var idAttrGroup = "#img-card-group-attr"
var idAttrName = "#id-group-attr-name"

var taHistoryCapas = new Dictionary();
var keyAttr = "attr";
var curAttr = "";
var keyComp = "comp";
var curComp = "";

var taComponentsName = new Dictionary();
var taComponentsDescription = new Dictionary();
var taAttributesName = new Dictionary();
var taAttributesDescription = new Dictionary();

var redColor = new Array(
    "#FF0000",
    "#FF5151",
    "#ff7575",
    "#FFB5B5",
    "#FFECEC",
    "#FFFFFF",
    "#CEFFCE",
    "#93FF93",
    "#28FF28"
);
function genarateCapaTable() {
    $("div[id=capabilities]").find("div[class='tab-pane active show']").attr("class", "tab-pane");
    taAttributesName.removeAll();
    taAttributesDescription.removeAll();
    $("h5[id=id-group-attr-name]").each(function (i) {
        taAttributesName.put(i.toString(), $(this).text());
    });
    $("input[id=id-group-attr-desc]").each(function (i) {
        taAttributesDescription.put(i.toString(), $(this).val());
    });
    taComponentsName.removeAll();
    taComponentsDescription.removeAll();
    $("h5[id=id-group-comp-name]").each(function (i) {
        taComponentsName.put(i.toString(), $(this).text());
    });
    $("input[id=id-group-comp-desc]").each(function (i) {
        taComponentsDescription.put(i.toString(), $(this).val());
    });

    $("thead tr").html("<th scope=\"col\">#</th>");
    for (var i = 0; i < taAttributesName.data.length; i++) {
        $("thead tr").append(" <th scope=\"col\">" + taAttributesName.data[i] + "</th>")
    }
    $("#id-capa-body").html("");
    // $("div[id=id-capability-tabs]").html("");
    var tabCount = 0;
    for (var i = 0; i < taComponentsName.data.length; i++) {
        // draw table
        $("#id-capa-body").append("<tr></tr>");
        $("#id-capa-body tr").eq(i).append("<th scope=\"row\">" + taComponentsName.data[i] + "</th>");
        for (var j = 0; j < taAttributesName.data.length; j++) {
            {
                $("#id-capa-body tr").eq(i).append("<td><a id=\"id-capa\" href=\"#"
                    + taComponentsName.data[i] + taAttributesName.data[j] + "\" comp=\""
                    + taComponentsName.data[i] + "\" attr=\"" + taAttributesName.data[j] + "\""
                    + " data-toggle=\"tab\" role = \"tab\" " + "onclick=\"showCurrentCapability(this)\""
                    + ">" + "0" + "</a></td>");
            }
            // draw table
            if ($("div[id=id-capability-tabs] div[id=" + taComponentsName.data[i] + taAttributesName.data[j] + "]").attr("id") == taComponentsName.data[i] + taAttributesName.data[j]) {
                $("a[attr=" + taAttributesName.data[j] + "][comp=" + taComponentsName.data[i] + "]")
                    .text($("div[id=id-capability-tabs]").find("div[id=" + taComponentsName.data[i] + taAttributesName.data[j] + "]").find("div[id=id-capa-card]").length);
                continue;
            }
            $("div[id=id-capability-tabs]").append($("#template-tab-capability").html());
            $("div[id=id-capability-tabs] div[id=cap] h4[id=lab]").text(taComponentsName.data[i] + " is " + taAttributesName.data[j]);
            $("div[id=id-capability-tabs] div[id=cap]").attr("id", taComponentsName.data[i] + taAttributesName.data[j]);

            tabCount++;
        }
    }
}
function downloadRisk() {
    $.ajax({
        url: '/project/make-risktable-xls',
        type: 'POST',
        data: JSON.stringify({
            Name: $('#project-name').val(),
            tableHTML: $("#risk").find("table").prop("outerHTML")
        }),
        async: false,
        cache: false,
        contentType: "application/json; charset=utf-8",
        success: function (returndata) {
            window.location.href = "project/download-risktable-xls?file=" + $('#project-name').val()+"_Risk.xls";
        },
        error: function (returndata) { }
    });
}

function genarateRiskTable() {
    taAttributesName.removeAll();
    taAttributesDescription.removeAll();
    $("h5[id=id-group-attr-name]").each(function (i) {
        taAttributesName.put(i.toString(), $(this).text());
    });
    $("input[id=id-group-attr-desc]").each(function (i) {
        taAttributesDescription.put(i.toString(), $(this).val());
    });
    taComponentsName.removeAll();
    taComponentsDescription.removeAll();
    $("h5[id=id-group-comp-name]").each(function (i) {
        taComponentsName.put(i.toString(), $(this).text());
    });
    $("input[id=id-group-comp-desc]").each(function (i) {
        taComponentsDescription.put(i.toString(), $(this).val());
    });

    $("thead tr").html("<th scope=\"col\">#</th>");
    for (var i = 0; i < taAttributesName.data.length; i++) {
        $("thead tr").append(" <th scope=\"col\">" + taAttributesName.data[i] + "</th>")
    }
    $("#id-risk-body").html("");
    // $("div[id=id-capability-tabs]").html("");
    var tabCount = 0;
    for (var i = 0; i < taComponentsName.data.length; i++) {
        $("#id-risk-body").append("<tr></tr>");
        $("#id-risk-body").children("tr").eq(i).append("<th scope=\"row\">" + taComponentsName.data[i] + "</th>");
        var maxSpan = 0;
        for (var j = 0; j < taAttributesName.data.length; j++) {
            {
                var totalrisk = 0;
                var showString = "";
                $("#id-risk-body").children("tr").eq(i).append("<td></td>");
                $("#id-capa-cards").children("div").each(function (ii) {
                    var name = $(this).children("h5").text();
                    if (taComponentsName.data[i] == $(this).children("h5").attr("comp") &&
                        taAttributesName.data[j] == $(this).children("h5").attr("attr")) {
                        showString = name;
                        totalrisk =
                            parseInt($(this).children("div[class=card-body]").find("input[name=name-fof" + name + "]:checked").val()) +
                            parseInt($(this).children("div[class=card-body]").find("input[name=name-impact" + name + "]:checked").val());
                        $("#id-risk-body tr td").eq(j).append("<div>" + (ii+1).toString() + " - "+ showString + "<div>");
                        $("#id-risk-body tr td").eq(j).children("div").eq(ii).attr("style", "background:" + redColor[8 - totalrisk]);
                        // <tr><td>
                    }
                });
            }
        }
    }
}

function createNewAttribute() {
    $(idAttrGroup).prepend($("#template-card-attribute").html());
    $("#attr-name").text($("#project-attr").val());
    $("#attr-name").attr("id", "id-group-attr-name");
    $("#attr-desc").attr("id", "id-group-attr-desc");
    $("#attr-tags").attr("id", "id-group-attr-tags");
}

function createNewComponent() {
    $("#img-card-group-comp").prepend($("#template-card-component").html());
    $("#comp-name").text($("#project-comp").val());
    $("#comp-name").attr("id", "id-group-comp-name");
    $("#comp-desc").attr("id", "id-group-comp-desc");
    $("#comp-tags").attr("id", "id-group-comp-tags");
}
function hideBody(dom) {
    if ($(dom).next().attr("hide") == "true") {
        $(dom).next().show("fast");
        $(dom).next().attr("hide", false);
    } else {
        $(dom).next().hide("fast");
        $(dom).next().attr("hide", true);
    }
}
function createNewCapability(dom) {
    $(dom).parent().parent().children("div[id=id-capa-cards]").prepend($("#template-card-capability").html());
    $("#capa-name").text($(dom).parent().children("div").children("input").val());
    $("#capa-name").attr("attr", curAttr);
    $("#capa-name").attr("comp", curComp);
    var name = $("#capa-name").text();
    $("#capa-name").parent().find("input[name=name-fof]").each(function (i) {
        var ran = Math.random().toString();
        $(this).attr("id", ran);
        $(this).attr("name", "name-fof" + $("#capa-name").text());
        $(this).next().attr("for", ran);
    });
    $("#capa-name").parent().find("input[name=name-impact]").each(function (i) {
        var ran = Math.random().toString();
        $(this).attr("id", ran);
        $(this).attr("name", "name-impact" + $("#capa-name").text());
        $(this).next().attr("for", ran);
    });
    $("#capa-name").attr("id", "id-group-capa-name");

    $("a[attr=" + curAttr + "][comp=" + curComp + "]").text($(dom).parent().parent().children("div[id=id-capa-cards]").children("div").length);
}

function removeCard(dom) {
    if (window.confirm('Confirm Remove')) {
        if ($(dom).attr("id") == ("id-capa-btn")) {
            var lenAfterRemove = $(dom).parent().parent().children("div").length - 1;
            $("a[attr=" + curAttr + "][comp=" + curComp + "]").text(lenAfterRemove);
        }
        $(dom).parent().remove();
    }
}

function EditProject() {
    $.ajax({
        url: '@Url.Action("ModifyProject")',
        type: 'POST',
        data: JSON.stringify({
            Name: $('#project-name').val(),
            Description: $('#project-desc').val(),
            Owners: $('#project-owners').val(),
            Editors: $('#project-editors').val(),
            Viewers: $('#project-viewers').val(),
            IsPublic: $('#project-is-public').val()
        }),
        async: true,
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (returndata) {
            window.location.href = "/project?id=" + max;
        },
        error: function (returndata) {
            alert();
        }
    });
}
