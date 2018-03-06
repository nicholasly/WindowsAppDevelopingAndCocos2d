#include "cocos2d.h"
using namespace std;

class PictureGame :public cocos2d::Layer
{
public:
	static cocos2d::Scene* createScene();
	virtual bool init();
	CREATE_FUNC(PictureGame);
	void initUI();
	void changeSize(cocos2d::Ref* pSender);
	bool idLarge;
};