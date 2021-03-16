export function getLocalObject(initialObject, name) {
    if (typeof name != "string") {
        throw "Object name is not of type string.";
    }

    return name
        .split(".")
        .reduce((accumulatedObject, key) =>
            (accumulatedObject && accumulatedObject[key]) ? accumulatedObject[key] : null,
            initialObject);
}

export function getGlobalObject(name) {
    const initialObject = window;
    return getLocalObject(initialObject, name);
}

export function getWindow() {
    return window;
}