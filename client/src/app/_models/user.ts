export interface User {
  username: string;
  token: string;
  photoUrl: string;
  knownAs: string;
  mentorOrMentee: string;
  roles: string[];
  yearsOfCareerExperience: number;
  careerInterests: string;
}
