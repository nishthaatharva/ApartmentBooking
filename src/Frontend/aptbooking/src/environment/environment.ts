import devEnv from "./env.dev";

const getApiUrl = () => {
    return devEnv.ResourceServer.apiUrl;
};

export default getApiUrl;