export const msalConfig = {
    auth: {
        clientId: process.env.REACT_APP_CLIENT_ID!,
        authority: process.env.REACT_APP_AUTHORITY!,
        redirectUri: process.env.REACT_APP_REDIRECT_URI!,
    },
};

export const loginRequest = {
    scopes: [process.env.REACT_APP_LOGIN_REQUEST_SCOPE!],
};
