export const msalConfig = {
    auth: {
        clientId: "1c936d23-884d-4dd3-81ff-67a4b19415f1",
        authority: "https://login.microsoftonline.com/cf9a707a-0335-48f2-b7d0-5bb01b8b4a94",
        redirectUri: "http://localhost:3000",
    },
};

export const loginRequest = {
    scopes: ["api://930bcc28-5810-49a6-ace5-eac0b2b0942b/access_as_user"],
};
