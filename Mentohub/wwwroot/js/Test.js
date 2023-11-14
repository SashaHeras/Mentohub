class Test {   
    constructor(config) {
        this.config = config;

        this.correctCounter = 0;

        this.answerCnt = 0;
        this.taskId = 0;
        this.taskMark = 0;
        this.orderNumber = 0;
        this.taskName = "";

        this.symbols = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L'];
        this.edittingHtmlCode = `                
            <div class="col">
                <div class="row row-content">
                    <div class="col col-lg-2">
                        <label>Task name:</label>
                    </div>
                    <div class="col">
                        <input id="taskname" type="text" class="form-control" placeholder="Enter name of task" />
                    </div>
                    <div class="col col-lg-2">
                        <label>Task mark:</label>
                    </div>
                    <div class="col">
                        <input id="taskmark" class="form-control" type="number" placeholder="Enter mark" style="width: 60%">
                    </div>
                    <div class="col">
                        <div style="text-align: right;">
                            <button style="width: 70%" id="DeleteTaskBtn" class="btn btn-danger" onclick="DeleteTask()">Delete task</button>
                        </div>
                    </div>
                </div>
                <div class="row row-content" id="testAnswers">
                
                </div>
                <div class="row row-content" id="buttons">

                </div>
            </div>
        `;
        this.creattingHtmlCode = `
                <table class="center" width="40%">
            <tr class="myTr">
                    <td style="padding-right: 6px" class="shortTd">
                    <label>Task name:</label>
                </td>
                <td>
                    <input id="taskname" class="form-control" type="text" placeholder="Enter name of task" width="350px" />
                </td>
            </tr>
                <tr class="myTr">
                    <td style="padding-right: 6px" class="shortTd">
                    <label>Task mark:</label>
                </td>
                <td>
                    <input id="taskmark" class="form-control" type="number" placeholder="Enter mark" width="25px" />
                </td>
            </tr>
                <tr class="myTr">
                    <td style="padding-right: 6px" class="shortTd">
                    <label>Answers count:</label>
                </td>
                <td>
                        <input id="ansCnt" class="form-control" type="number" placeholder="Enter answers number" width="25px" />
                </td>
            </tr>
                <tr class="myTr">
                <td style="padding-right: 6px">
                </td>
                <td style="text-align: right">
                        <button id="SaveTaskBtn" class="btn btn-light" onclick="SaveNewTaskData()">Submit</button>
                </td>
            </tr>
        </table>
            <div id="testAnswers" width="100%">
                        <div id="taskAnswers" class="center-div">

                        </div>
                        <div id="buttons" class="center-div">

                        </div>
                    </div>
    `;

        this.answerCnt = 0;

        this.currentTaskAnswers = [];
    }

    initEdit() {
        let con = document.getElementById("myContext");
        con.innerHTML = this.edittingHtmlCode;

        let saveTestBtn = document.querySelector('button#SaveTest');
        saveTestBtn.addEventListener("click", () => this.SaveTest());

        this.GenerateTasksList();
    }

    DeleteTask() {
        console.log(taskId);
        $.ajax({
            url: "/Test/DeleteTask/" + taskId,
            type: "DELETE",
            dataType: "json",
            success: function (result) {
                if (result == true) {
                    let con = document.getElementById("myContext");
                    con.innerHTML = this.edittingHtmlCode;

                    this.GenerateTasksList();
                }
            }
        });
    }

    // Saving data of new task (mark, name, count answers)
    SaveNewTaskData() {
        taskMark = document.getElementById("taskmark").value;
        taskName = document.getElementById("taskname").value;
        answerCnt = document.getElementById("ansCnt").value;

        this.CreateAnswersFields();
    }

    SaveTest() {
        let name = document.getElementById("testname").value;

        let formData = new FormData();

        formData.append("testName", name);
        formData.append("testId", this.config.testId);
        formData.append("courseId", this.config.courseId);

        $.ajax({
            url: "/Test/SaveTest",
            type: "POST",
            data: formData,
            dataType: "json",
            contentType: false,
            processData: false,
            success: function (result) {
                if (result != 0) {
                    this.config.testId = result;
                    alert("Test name edited!");
                }
            }
        });
    }

    // Method for onclick on test element
    TestElementClick(div, id) {
        this.ChangeActiveDiv(div);
        this.GenerateTask(id);
    }

    GenerateTasksList() {
        let main = document.getElementById("tasksList");
        main.innerHTML = '';

        let myUrl = "/Test/GetTasks/" + this.config.testId;
        $.ajax({
            url: myUrl,
            type: "GET",
            dataType: "json",
            success: function (result) {
                let inside = document.createElement("div");
                inside.className = "insideDiv";

                for (let i = 0; i < result.length; i++) {
                    let div = document.createElement("div");
                    div.className = "testElement";
                    div.textContent = result[i].orderNumber;
                    div.addEventListener("click", () => this.TestElementClick(div, result[i].id));
                    inside.appendChild(div);
                }

                let createNewButton = document.createElement("button");
                createNewButton.className = "btn btn-primary";
                createNewButton.innerHTML = `<i class="bi bi-plus-circle"></i>`;
                createNewButton.addEventListener("click", this.ChangeOnCreateForm());

                inside.appendChild(createNewButton);
                main.appendChild(inside);
            }
        });
    }

    CreateAnswersFields() {
        let block = document.getElementById("taskAnswers");
        let buttons = document.getElementById("buttons");
        block.innerHTML = '';
        buttons.innerHTML = '';

        for (let i = 0; i < answerCnt; i++) {
            let div = document.createElement("div");
            div.setAttribute("name", "answerBlock");
            div.setAttribute("class", "answerBlock");
            div.id = "answerBlock";

            let cor = document.createElement("input");
            cor.setAttribute("type", "checkbox");
            cor.id = "correct" + (i + 1);

            div.appendChild(cor);

            let ans = document.createElement("input");
            ans.setAttribute("type", "text");
            ans.className = "form-control answerField";
            ans.setAttribute("placeholder", "Enter answer");
            ans.setAttribute("width", "250px");
            ans.id = "answer" + 1;

            div.appendChild(ans);

            block.appendChild(div);
        }

        let btn = document.createElement("button");

        btn.addEventListener("click", () => this.SaveNewTaskAnswers());

        btn.className = "btn btn-light";
        btn.textContent = "Submit";

        buttons.appendChild(btn);
    }

    SaveNewTaskAnswers() {
        let allAnswers = [];
        let allChecked = [];

        // Reading all entered answers
        let answers = document.getElementsByName("answerBlock");
        for (let i = 0; i < this.answerCnt; i++) {
            let c = 0;
            for (const child of answers[i].children) {
                if (c == 0) {
                    allChecked.push(child.checked);
                }
                else if (c == 1) {
                    allAnswers.push(child.value.replace(',', '|'));
                }
                c++;
            }
        }

        // Checking is a correct answers
        if (allChecked.find(function (e) { return e == true }) == undefined) {
            alert("Select true variant!");
        }
        else {
            let formData = new FormData();

            this.orderNumber = document.querySelectorAll('#tasksList .testElement').length;
            this.orderNumber++;

            formData.append("answers", allAnswers);
            formData.append("checked", allChecked);
            formData.append("orderNumber", this.orderNumber);
            formData.append("taskName", this.taskName);
            formData.append("taskMark", this.taskMark);
            formData.append("testId", this.config.testId);

            $.ajax({
                url: "/Test/SaveAnswers",
                type: "POST",
                data: formData,
                dataType: "json",
                contentType: false,
                processData: false,
                success: function (result) {
                    this.Cleaning();
                    this.GenerateTasksList();
                }
            });
        }
    }

    ChangeOnCreateForm() {
        if (isEditting == false) {
            return;
        }

        document.getElementById("myContext").innerHTML = this.creattingHtmlCode;
        this.configisEditting = false;
    }

    GenerateTask(id) {
        if (this.config.isEditting == false) {
            document.getElementById("myContext").innerHTML = "";
            this.config.isEditting = true;
            document.getElementById("myContext").innerHTML = this.edittingHtmlCode;
        }

        let myUrl = "/Test/GetTask/" + id;
        $.ajax({
            url: myUrl,
            type: "GET",
            dataType: "json",
            success: function (result) {
                document.getElementById("taskmark").value = "";
                document.getElementById("taskname").value = "";
                document.getElementById("taskmark").value = result.mark;
                document.getElementById("taskname").value = result.name;
            }
        });

        this.taskId = id;
        this.GenerateAnswers(id);
    }

    DeleteTask() {
        console.log(this.taskId);
        $.ajax({
            url: "/Test/DeleteTask/" + this.taskId,
            type: "DELETE",
            dataType: "json",
            success: function (result) {
                if (result == true) {
                    let con = document.getElementById("myContext");
                    con.innerHTML = this.edittingHtmlCode;

                    this.GenerateTasksList();
                }
            }
        });
    }

    // Generation of test elements
    GenerateTasksList() {
        let main = document.getElementById("tasksList");
        removeAllChildElements(main);

        let myUrl = "/Test/GetTasks/" + this.config.testId;
        $.ajax({
            url: myUrl,
            type: "GET",
            dataType: "json",
            success: function (result) {
                let inside = document.createElement("div");
                inside.className = "insideDiv";

                for (let i = 0; i < result.length; i++) {
                    let div = document.createElement("div");
                    div.className = "testElement";
                    div.textContent = result[i].orderNumber;
                    div.addEventListener("click", () => this.TestElementClick(div, result[i].id));
                    inside.appendChild(div);
                }

                let createNewButton = document.createElement("button");
                createNewButton.className = "btn btn-primary";
                createNewButton.innerHTML = `<i class="bi bi-plus-circle"></i>`;
                createNewButton.addEventListener("click", this.ChangeOnCreateForm());

                inside.appendChild(createNewButton);
                main.appendChild(inside);
            }
        });
    }

    removeAllChildElements(block) {
        while (block.firstChild) {
            block.removeChild(block.lastChild);
        }
    }

    // Function for cleaning place for new task
    Cleaning() {
        let block = document.getElementById("testAnswers");
        block.innerHTML = "";

        document.getElementById("taskmark").value = "";
        document.getElementById("taskname").value = "";
    }

    GenerateAnswers(taskId) {
        let block = document.getElementById("testAnswers");
        let buttons = document.getElementById("buttons");
        this.correctCounter = 0;

        this.removeAllChildElements(block);
        this.removeAllChildElements(buttons);

        console.log(taskId);

        const thisUrl = `/Test/GetAnswersForEditting/${taskId}`;
        $.ajax({
            url: thisUrl,
            type: "GET",
            dataType: "json",
            success: function (result) {
                this.currentTaskAnswers = result;

                console.log(this.currentTaskAnswers);

                this.taskId = this.currentTaskAnswers[0].taskId;
                console.log(this.currentTaskAnswers);

                result.forEach(answer => {
                    const div = document.createElement("div");
                    div.setAttribute("name", "answerBlock");
                    div.className = "answerBlock";
                    div.id = `answerBlock${answer.id}`;

                    const cor = document.createElement("input");
                    cor.setAttribute("type", "checkbox");
                    cor.checked = answer.isCorrect;
                    cor.id = `correct${answer.id}`;
                    div.appendChild(cor);

                    const ans = document.createElement("input");
                    ans.setAttribute("type", "text");
                    ans.setAttribute("placeholder", "Enter answer");
                    ans.className = "answerField form-control";
                    ans.setAttribute("value", answer.name);
                    ans.setAttribute("width", "450px");
                    ans.id = `answer${answer.id}`;
                    div.appendChild(ans);

                    const deleteAnswer = document.createElement("div");
                    deleteAnswer.setAttribute("onclick", `DeleteAnswer('${answer.id}')`);
                    deleteAnswer.setAttribute("class", "dot");
                    deleteAnswer.innerHTML = `<i class="bi bi-trash-fill"></i>`;
                    div.appendChild(deleteAnswer);

                    block.appendChild(div);
                });

                let btnSub = document.createElement("button");
                btnSub.setAttribute("onclick", "SaveAnswers()");
                btnSub.setAttribute("style", "margin-left: 25%;");
                btnSub.className = "btn btn-success";
                btnSub.textContent = "Submit";

                let btnAdd = document.createElement("button");
                btnAdd.setAttribute("onclick", "AddAnswerField()");
                btnAdd.className = "btn btn-light";
                btnAdd.textContent = "Add new answer";

                buttons.appendChild(btnAdd);
                buttons.appendChild(btnSub);
            }
        });
    }

    SaveAnswers() {
        let allAnswers = [];
        let allChecked = [];
        let allIds = [];

        // Reading all entered answers
        let answers = document.getElementsByName("answerBlock");
        for (let i = 0; i < answers.length; i++) {
            let c = 0;
            allIds.push(answers[i].id.replace("answerBlock", ""));
            for (const child of answers[i].children) {
                if (c == 0) {
                    allChecked.push(child.checked);
                }
                else if (c == 1) {
                    allAnswers.push(child.value.replace(',', '|'));
                }
                c++;
            }
        }
        // Checking is a correct answers
        if (allChecked.some(item => item === true) == false) {
            alert("Select true variant!");
        }
        else {
            taskMark = document.getElementById("taskmark").value;
            taskName = document.getElementById("taskname").value;

            let formData = new FormData();

            formData.append("answers", allAnswers);
            formData.append("checked", allChecked);
            formData.append("ids", allIds);
            formData.append("taskName", this.taskName);
            formData.append("taskMark", this.taskMark);
            formData.append("taskId", this.config.taskId);
            formData.append("testId", this.config.testId);
            $.ajax({
                url: "/Test/SaveEdittedAnswers",
                type: "POST",
                data: formData,
                dataType: "json",
                contentType: false,
                processData: false,
                success: function (result) {

                }
            });
        }
        this.correctCounter = 0;
    }

    AddAnswerField() {
        let block = document.getElementById("testAnswers");
        let buttons = document.getElementById("buttons");

        let div = document.createElement("div");
        div.setAttribute("name", "answerBlock");
        div.className = "answerBlock";
        div.id = "answerBlock" + this.symbols[this.correctCounter];

        let cor = document.createElement("input");
        cor.setAttribute("type", "checkbox");
        cor.id = "correct" + this.correctCounter;
        this.correctCounter++;

        div.appendChild(cor);

        let ans = document.createElement("input");
        ans.setAttribute("type", "text");
        ans.setAttribute("placeholder", "Enter answer");
        ans.className = "answerField";
        ans.setAttribute("width", "450px");
        ans.id = "answer" + 1;

        div.appendChild(ans);

        let deleteAnswer = document.createElement("div");
        deleteAnswer.setAttribute("onclick", "DeleteAnswer('" + this.symbols[this.correctCounter] + "')");
        deleteAnswer.setAttribute("class", "dot");
        deleteAnswer.innerHTML = "-"

        div.appendChild(deleteAnswer);

        block.appendChild(div);
    }

    DeleteAnswer(id) {
        let deletedElem = document.getElementById("answerBlock" + id);
        deletedElem.remove();

        $.ajax({
            url: "/Test/DeleteAnswer",
            type: "POST",
            data: {
                answerId: id
            },
            dataType: "json",
            contentType: false,
            processData: false,
            success: function (result) {
                if (result != false) {
                    this.currentTaskAnswers = result;
                }
            }
        });
    }
}