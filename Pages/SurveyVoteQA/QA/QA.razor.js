export function Init(id, component) {

    let e = document.getElementById(`other-${id}`)
    let value = e.value;

    component.invokeMethodAsync("returnValue", value, id)
   
}

export async function GetIp(component) {
    const response = await fetch("https://api64.ipify.org?format=json");
    const data = await response.json();

    component.invokeMethodAsync("GetIpReturn", data.ip)
}