// a class is being used here instead of an interface to take advantage of

import { User } from "./user";

// the fact that a constructor can be used to initialize some of the properties
export class UserParams {
  mentorOrMentee: string;
  minYearsOfCareerExperience = 0;
  maxYearsOfCareerExperience = 50;
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'lastActive';

  constructor(user: User) {
    this.mentorOrMentee = user.mentorOrMentee === 'Mentor' ? "Mentee" : 'Mentor';
  }

}
