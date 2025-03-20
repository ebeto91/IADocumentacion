// Get the root element

let r = document.querySelector(':root');


const listCssVariables = {
    'h1': {
        initialSize: 2,
        valueChange: 0.5,
        maxValue: 3
    },
    'h2': {
        initialSize: 1.5,
        valueChange: 0.5,
        maxValue: 2.5
    },
    'h3': {
        initialSize: 1.2,
        valueChange: 0.5,
        maxValue: 2.2
    },
    'h4': {
        initialSize: 1,
        valueChange: 0.5,
        maxValue: 2
    },
    'h5': {
        initialSize: 0.8,
        valueChange: 0.5,
        maxValue: 1.8
    },
    'h6': {
        initialSize: 0.9,
        valueChange: 0.2,
        maxValue: 1.2
    },
    'p': {
        initialSize: 1,
        valueChange: 0.4,
        maxValue: 1.8
    },
    'div': {
        initialSize: 0.75,
        valueChange: 0.2,
        maxValue: 2.7
    },
    'span': {
        initialSize: 1,
        valueChange: 0.4,
        maxValue: 1.8
    },
    'label': {
        initialSize: 0.8,
        valueChange: 0.2,
        maxValue: 1.2
    },
    'a': {
        initialSize: 0.9,
        valueChange: 0.2,
        maxValue: 1.2
    },
    'input-submit': {
        initialSize: 0.6,
        valueChange: 0.2,
        maxValue: 1
    },
    'i': {
        initialSize: 1.2,
        valueChange: 0.2,
        maxValue: 1.6
    },
    'table-text': {
        initialSize: 0.875,
        valueChange: 0.02,
        maxValue: 0.915
    }
    ,
    'table-text': {
        initialSize: 0.875,
        valueChange: 0.02,
        maxValue: 0.915
    }


};

document.addEventListener("DOMContentLoaded", function (event) {
    let listItem = localStorage.getItem("cssFontSize");
    if (listItem && listItem != "") {
        let listCssVariables = Array.from(JSON.parse(listItem));
        for (const [key, value] of Object.entries(listCssVariables)) {
            setPropertyFontSize(`--font-size-${value.key}`, `${value.value}rem`)
        }

    }
});

function plusFontSize() {

    // Get the styles (properties and values) for the root
    let rs = getComputedStyle(r);
    let list = []

    for (const [key, valueList] of Object.entries(listCssVariables)) {
        let variableValue = rs.getPropertyValue(`--font-size-${key}`);
        let floatParsed = parseFloat(variableValue);
        if (floatParsed < valueList.maxValue) {
            let calculateValue = Number(parseFloat(floatParsed + valueList.valueChange).toFixed(2));
            list.push({
                key: key,
                value: calculateValue
            })
            setPropertyFontSize(`--font-size-${key}`, `${calculateValue}rem`)
        } else {
            list.push({
                key: key,
                value: floatParsed
            })
        }
    }
    localStorage.setItem("cssFontSize", JSON.stringify(list));
}

function minusFontSize() {
    // Get the styles (properties and values) for the root
    let rs = getComputedStyle(r);
    let list = []
    for (const [key, valueList] of Object.entries(listCssVariables)) {
        let variableValue = rs.getPropertyValue(`--font-size-${key}`);
        let floatParsed = parseFloat(variableValue);
        if (floatParsed > valueList.initialSize) {
            let calculateValue = Number(parseFloat(floatParsed - valueList.valueChange).toFixed(2));
            list.push({
                key: key,
                value: calculateValue
            })
            setPropertyFontSize(`--font-size-${key}`, `${calculateValue}rem`)
        } else {
            list.push({
                key: key,
                value: valueList.initialSize
            })
            setPropertyFontSize(`--font-size-${key}`, `${valueList.initialSize}rem`)
        }
    }
    localStorage.setItem("cssFontSize", JSON.stringify(list));
}

// Create a function for setting a variable value
function setPropertyFontSize(key, value) {
    // Set the value of variable --blue to another value (in this case "lightblue")
    r.style.setProperty(key, value);
}