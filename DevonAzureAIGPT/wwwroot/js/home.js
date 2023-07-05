
let historyMessages = [];
let shotExamples = [];
let conversations = [];
let userInput = "";
var coll = document.getElementsByClassName("collapsible");
var i;

for (i = 0; i < coll.length; i++) {
    coll[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var content = this.nextElementSibling;
        if (content.style.maxHeight) {
            content.style.maxHeight = null;
        } else {
            content.style.maxHeight = content.scrollHeight + "px";
        }
    });
}

const container = document.getElementById('responseDiv');
const observer = new MutationObserver(() => {
    container.scrollTop = container.scrollHeight;
});
observer.observe(container, { childList: true });

$(document).ready(function () {

    $("#toggleLeftPanel").on("click", function () {
        if ($("#toggleLeftPanel").text() === "Hide Assistant Setup")
            $("#toggleLeftPanel").text("Show Assistant Setup");
        else
            $("#toggleLeftPanel").text("Hide Assistant Setup");
        $("#leftPanel").toggleClass("transformed");
        $("#rightPanel").toggleClass("transformed");
    });

    $('.input-text').keypress(function (e) {
        if (isNaN(this.value + "" + String.fromCharCode(e.charCode))) return false;
    }).on("cut copy paste", function (e) {
        e.preventDefault();
    });

    $("#modelDropdowm").change(function (e) {
        if ($("#modelDropdowm").val() == "text-davinci-003") {
            $("#temperatureText").val(1);
            $("#topPText").val(0.5);
        }
        else {
            $("#temperatureText").val(0.7);
            $("#topPText").val(0.7);
        }
    });

    userInput = $("#promptValue").val();
    const textArea = document.getElementById("promptValue");
    const myButton = document.getElementById("generateResponse");
    myButton.disabled = true;

    textArea.addEventListener("input", function () {
        if (textArea.value.trim() !== "") {
            myButton.disabled = false;
        } else {
            myButton.disabled = true;
        }
    });

    $("#addShotMessage").click(function () {
        var innerHTML = generateShotExample();
        $(".shotMessageContainer").append(innerHTML);
        $('.shotMessageAssistant').on("focusout", function () {
            readShotMessagesWithHistory();
        });
    });

    $('#promptValue').on('keydown', (event) => {
        if (event.keyCode === 13) {
            event.preventDefault(); // prevent default behavior (e.g. inserting a new line)
            generateAIResponse();
        }
    });

    $("#generateResponse").click(function () {

        generateAIResponse();
    })

    $("#clearResponse").click(function () {
        document.getElementById("responseDiv").innerHTML = "";
        historyMessages = [];
        conversations = [];
        readShotMessagesWithHistory();
    })
});

function generateAIResponse() {
    userInput = $("#promptValue").val();
    $('#generateResponse').prop('disabled', true);
    setTimeout(() => {
        $("#spinner").show();
    }, 100)

    conversations = shotExamples.concat(historyMessages);

    var message = $("#sytemMessage").val();

    if ($("#txtfieldInfo").val()) {
        message = message + "\n" + $("#txtfieldInfo").val();
    }

    var gptconversation = {
        UserInput: userInput,
        SystemMessage: message,
        ChatHistories: conversations
    }

    var aiRequestParameters = {
        ModelName: $("#modelDropdowm").val(),//EOLgpt35,text-davinci-003
        Temperature: $("#temperatureText").val(),
        TopP: $("#topPText").val()
    }

    $.ajax({
        url: "../Home/GetResponseFromAI/",
        type: "POST",
        data: { conversations: gptconversation, aiRequestParameters: aiRequestParameters },
        dataType: "json",
        success: function (data) {
            onSuccess(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}

function onSuccess(data) {
    $("#responseDiv").append('<div class="user-message-div"><div class="user-message d-flex flex-row-reverse p-2 bd-highlight shadow-sm mt-3">' + userInput + '</div></div>');
    $("#responseDiv").append('<div class="assistant-message-div"><div class="assistant-message d-flex p-2 bd-highlight shadow-sm mt-3">' + data + '</div>');

    historyMessages.push({
        User: userInput,
        Assistant: data
    });

    $("#promptValue").val("");
    setTimeout(() => {
        $("#spinner").hide();
    }, 100)
}

function generateShotExample() {
    // create the main container
    const mainContainer = document.createElement("div");
    mainContainer.classList.add("shot-example");

    // create the user input field
    const userContainer = `<div class="field-control">
        <label for="shotMessageUser">User</label>
        <textarea class="shotMessageUser field user-input" rows="2" cols="65"></textarea>
    </div>`;

    // create the assistant input field
    const assistantContainer = `<div class="field-control">
        <label for="shotMessageAssistant" class="label">Assistant</label>
        <textarea class="shotMessageAssistant field assistant-input" rows="2" cols="65"></textarea>
    </div>`;

    // append the heading and input fields to the main container
    mainContainer.innerHTML = userContainer + assistantContainer;

    // return the generated HTML code as a string
    return mainContainer.outerHTML;
}

function readShotMessagesWithHistory() {
    shotExamples = [];
    const shotMessageContainers = document.querySelectorAll('.shot-example');
    for (let i = 0; i < shotMessageContainers.length; i++) {
        const container = shotMessageContainers[i];
        const textareas = container.querySelectorAll('textarea');
        if (textareas[0].value != '' && textareas[1].value != '') {

            shotExamples.push({
                User: textareas[0].value,
                Assistant: textareas[1].value
            });
        }
    }
    conversations = shotExamples.concat(historyMessages);
}
