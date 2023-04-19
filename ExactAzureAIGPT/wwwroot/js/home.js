
let historyMessages = [];
let shotExamples = [];
let conversations = [];

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
    const userInput = $("#promptValue").val();
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

    $("#generateResponse").click(function () {

        myButton.disabled = true;
        setTimeout(() => {
            $("#spinner").show();
        }, 100)

        conversations = shotExamples.concat(historyMessages);

        var message = $("#sytemMessage").val() + "\n Here are the mappings:" + $("#txtfieldInfo").val();

        $.ajax({
            url: "../Home/GetResponse/",
            type: "POST",
            data: { conversations: conversations, userInput: userInput, systemMessage: message },
            dataType: "json",
            success: function (data) {
                onSuccess(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown);
            }
        });

    })

    $("#clearResponse").click(function () {
        document.getElementById("responseDiv").innerHTML = "";
        historyMessages = [];
        readShotMessagesWithHistory();
    })
});

function onSuccess(data) {
    $("#responseDiv").append('<div class="userMessage d-flex flex-row-reverse p-2 bd-highlight shadow-sm mt-3">' + userInput + '</div>');
    $("#responseDiv").append('<div class="assistantMessage d-flex p-2 bd-highlight shadow-sm mt-3">' + data + '</div>');

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
    mainContainer.classList.add("mb-3", "shot-example");

    // create the user input field
    const userContainer = `
                                            <div class="form-group">
                                              <label for="shotMessageUser" class="form-label">User:</label>
                                              <textarea class="shotMessageUser form-control user-input" rows="2" cols="65"></textarea>
                                            </div>
                                          `;

    // create the assistant input field
    const assistantContainer = `
                                            <div class="form-group">
                                              <label for="shotMessageAssistant" class="form-label">Assistant:</label>
                                              <textarea class="shotMessageAssistant form-control assistant-input" rows="2" cols="65"></textarea>
                                            </div>
                                          `;

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
