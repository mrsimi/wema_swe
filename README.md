# wema_swe

### Process flow for onboarding: 
1. User initiates onboarding usnig the `onboarding/initiate` providing all the necessary details. In an even that the lga is not in the state and error response is returned asides the other error resposnes that could arise from the request itself. It then returns with a countdown and verificationReference 
2. user completes the onboarding process using the `onboarding/complete` method where the verificationReference from `onboarding/initiate` and the OTP of `12345` is provided. 




Notes: Because the OTP service is mocked a test otp of 12345 is used to completed the onboarding process. 
